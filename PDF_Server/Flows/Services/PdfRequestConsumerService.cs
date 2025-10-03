using Microsoft.Extensions.DependencyInjection;
using Confluent.Kafka;
using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;
using System.Text.Json;

namespace PDF_Server.Flows.Services
{
    public class PdfRequestConsumerService : BackgroundService
    {

        private readonly IProducer<Null, string> _kafkaProducer;
        private readonly ILogStorageService _logStorageService;

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public PdfRequestConsumerService(
                    IProducer<Null, string> kafkaProducer, ILogStorageService logStorageService, 
                    IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _kafkaProducer = kafkaProducer;
            _logStorageService = logStorageService;

            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "pdf_server_consumer",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_configuration["Kafka:PdfRequestTopic"]);

            while (!stoppingToken.IsCancellationRequested)
            {
                string correlationId = "";
                DateTime timestamp = DateTime.UtcNow;

                try
                {
                    var result = consumer.Consume(stoppingToken);
                    var request = JsonSerializer.Deserialize<PdfRequestDto>(result.Message.Value);
                    correlationId = request.CorrelationId;

                    await LogAsync(correlationId, "Datos recibidos de Kafka correctamente.", stoppingToken);

                    Console.WriteLine($"Recibido: {JsonSerializer.Serialize(request)}");
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var pdfGenerator = scope.ServiceProvider.GetRequiredService<IPdfGenerator>();
                        try
                        {
                            await pdfGenerator.GenerateCustomerReportsAsync(request);
                            await LogAsync(correlationId, "Consulta a la base de datos y creación de PDF exitosa.", stoppingToken);
                        }
                        catch (Exception exDb)
                        {
                            await LogAsync(correlationId, $"Error al consultar la base de datos o crear el PDF: {exDb.Message}", stoppingToken);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
                catch (Exception ex)
                {
                    await LogAsync(correlationId, $"Error al recibir datos de Kafka: {ex.Message}", stoppingToken);
                }
            }
        }
        private async Task LogAsync(string correlationId, string message, CancellationToken stoppingToken)
        {
            var log = new ProcessLogDto
            {
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow,
                Message = message
            };

            var logJson = JsonSerializer.Serialize(log);
            var logTopic = _configuration["Kafka:LogTopic"];
            await _kafkaProducer.ProduceAsync(logTopic, new Message<Null, string> { Value = logJson }, stoppingToken);
            await _logStorageService.SaveLogAsync(log);
        }
    }
}
