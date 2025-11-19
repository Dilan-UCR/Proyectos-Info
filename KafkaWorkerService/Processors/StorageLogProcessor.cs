using Confluent.Kafka;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Models;
using System.Text.Json;

namespace KafkaWorkerService.Processors;

public class StorageLogProcessor : IMessageProcessor
{
    private readonly IRepository<StorageLog> _storageRepository;
    private readonly IRepository<KafkaLog> _kafkaLogRepository;
    private readonly ILogger<StorageLogProcessor> _logger;

    public string TopicType => "logs-storage";

    public StorageLogProcessor(
        IRepository<StorageLog> storageRepository,
        IRepository<KafkaLog> kafkaLogRepository,
        ILogger<StorageLogProcessor> logger)
    {
        _storageRepository = storageRepository;
        _kafkaLogRepository = kafkaLogRepository;
        _logger = logger;
    }

    public async Task ProcessAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var storageLog = JsonSerializer.Deserialize<StorageLog>(message.Message.Value, options)
                ?? throw new JsonException("No se pudo deserializar el mensaje de Storage");

            await _storageRepository.AddAsync(storageLog);
            await _storageRepository.SaveChangesAsync();

            var kafkaLog = new KafkaLog
            {
                LogType = LogType.Storage,
                Topic = message.Topic,
                Message = message.Message.Value,
                Status = "Success",
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(kafkaLog);
            await _kafkaLogRepository.SaveChangesAsync();

            _logger.LogInformation("Storage procesado: CorrelationId={CorrelationId}, Level={Level}", 
                storageLog.CorrelationId, storageLog.Level);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando Storage log desde tópico {Topic}", message.Topic);
            
            var failureLog = new KafkaLog
            {
                LogType = LogType.Storage,
                Topic = message.Topic,
                Message = message.Message.Value,
                Status = "Failed",
                ErrorDetails = ex.Message,
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(failureLog);
            await _kafkaLogRepository.SaveChangesAsync();

            throw;
        }
    }
}
