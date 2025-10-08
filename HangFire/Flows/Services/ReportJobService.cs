using Hangfire;
using SERVERHANGFIRE.Flows.Services.Interfaces;
using SERVERHANGFIRE.Flows.DTOs;

namespace SERVERHANGFIRE.Flows.Services
{
   
    public class ReportJobService : IReportJobService
    {
        private readonly IHttpClientService _httpClient;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<ReportJobService> _logger;
 
        public ReportJobService(
            IHttpClientService httpClient,
            IKafkaProducerService kafkaProducer,
            ILogger<ReportJobService> logger)
        {
            _httpClient = httpClient;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessReportRequest(int customerId, DateTime startDate, DateTime endDate, string correlationId, List<int> products)
        {
            var log = new LogRequestDto
            {
                CorrelationId = correlationId,
                Service = "HangfireServer",
                Endpoint = "ProcessReportRequest",
                Timestamp = DateTime.UtcNow,
                Payload = $"CustomerId: {customerId}, StartDate: {startDate:yyyy-MM-dd}, EndDate: {endDate:yyyy-MM-dd}",
                Success = false
            };
            log.Payload += $", Productos: {string.Join(", ", products)}";

            try
            {
                _logger.LogInformation("🔄 Iniciando procesamiento. CorrelationId={CorrelationId}", correlationId);

                // Crear request para el PDF server
                var pdfRequest = new PdfRequestDto
                {
                    CustomerId = customerId,
                    StartDate = startDate,
                    EndDate = endDate,
                    CorrelationId = correlationId,
                    Products = products
                };

                // Enviar solicitud al PDF server (simulado)
                var success = await _httpClient.SendReportRequestAsync(pdfRequest);

                log.Success = success;
                log.Payload += $", PDFServerResponse: {(success ? "Success" : "Failed")}";

                // Enviar log a Kafka
                await _kafkaProducer.SendLogAsync(log);

                if (success)
                {
                    _logger.LogInformation("✅ Procesamiento completado. CorrelationId={CorrelationId}", correlationId);
                }
                else
                {
                    _logger.LogWarning("⚠ Procesamiento completado con errores. CorrelationId={CorrelationId}", correlationId);
                }
            }
            catch (Exception ex)
            {
                log.Payload += $", Error: {ex.Message}";
                await _kafkaProducer.SendLogAsync(log);
                _logger.LogError(ex, "❌ Error inesperado. CorrelationId={CorrelationId}", correlationId);
                throw; // Hangfire manejará el reintento
            }
        }
    }
}