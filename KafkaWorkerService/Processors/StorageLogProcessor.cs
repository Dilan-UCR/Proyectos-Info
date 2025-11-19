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
            var storageLog = JsonSerializer.Deserialize<StorageLog>(message.Value)
                ?? throw new JsonException("No se pudo deserializar el mensaje de Storage");

            // Patrón UPSERT: Buscar por CorrelationId
            var existingStorage = await _storageRepository.GetByPredicateAsync(s => s.CorrelationId == storageLog.CorrelationId);
            
            if (existingStorage != null)
            {
                // Actualizar registro existente
                existingStorage.Service = storageLog.Service;
                existingStorage.Endpoint = storageLog.Endpoint;
                existingStorage.Timestamp = storageLog.Timestamp;
                existingStorage.Payload = storageLog.Payload;
                existingStorage.Success = storageLog.Success;
                await _storageRepository.UpdateAsync(existingStorage);
            }
            else
            {
                // Crear nuevo registro
                await _storageRepository.AddAsync(storageLog);
            }

            await _storageRepository.SaveChangesAsync();

            var kafkaLog = new KafkaLog
            {
                LogType = LogType.Storage,
                Topic = message.Topic,
                Message = message.Value,
                Status = "Success",
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(kafkaLog);
            await _kafkaLogRepository.SaveChangesAsync();

            _logger.LogInformation("Storage procesado: CorrelationId={CorrelationId}, Éxito={Success}", 
                storageLog.CorrelationId, storageLog.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando Storage log desde tópico {Topic}", message.Topic);
            
            var failureLog = new KafkaLog
            {
                LogType = LogType.Storage,
                Topic = message.Topic,
                Message = message.Value,
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
