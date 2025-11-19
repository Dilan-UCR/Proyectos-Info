using KafkaWorkerService.Interfaces;

namespace KafkaWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IKafkaConsumer _kafkaConsumer;

    public Worker(ILogger<Worker> logger, IKafkaConsumer kafkaConsumer)
    {
        _logger = logger;
        _kafkaConsumer = kafkaConsumer;
    } 
 
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Iniciando Kafka Consumer Worker Service");
            
            var topics = new[]
            {
                "logs-email",
                "logs-hangfire",
                "logs-pdf",
                "logs-storage",
                "messages-logs"
            };

            await _kafkaConsumer.StartAsync(topics, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Worker Service cancelado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en el Worker Service");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deteniendo Worker Service");
        await _kafkaConsumer.StopAsync();
        await base.StopAsync(cancellationToken);
    }
}
