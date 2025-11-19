using Confluent.Kafka;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Models;
using System.Text.Json;

namespace KafkaWorkerService.Processors;

public class HangfireLogProcessor : IMessageProcessor
{
    private readonly IRepository<HangfireLog> _hangfireRepository;
    private readonly IRepository<KafkaLog> _kafkaLogRepository;
    private readonly ILogger<HangfireLogProcessor> _logger;

    public string TopicType => "logs-hangfire";

    public HangfireLogProcessor(
        IRepository<HangfireLog> hangfireRepository,
        IRepository<KafkaLog> kafkaLogRepository,
        ILogger<HangfireLogProcessor> logger)
    {
        _hangfireRepository = hangfireRepository;
        _kafkaLogRepository = kafkaLogRepository;
        _logger = logger;
    }

    public async Task ProcessAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            var hangfireLog = JsonSerializer.Deserialize<HangfireLog>(message.Value)
                ?? throw new JsonException("No se pudo deserializar el mensaje de Hangfire");

            // UPSERT: Actualizar si existe, insertar si no
            var existingLog = _hangfireRepository.Query()
                .FirstOrDefault(h => h.CorrelationId == hangfireLog.CorrelationId);

            if (existingLog != null)
            {
                // Actualizar registro existente
                existingLog.Service = hangfireLog.Service;
                existingLog.Endpoint = hangfireLog.Endpoint;
                existingLog.Timestamp = hangfireLog.Timestamp;
                existingLog.Payload = hangfireLog.Payload;
                existingLog.Success = hangfireLog.Success;
                await _hangfireRepository.UpdateAsync(existingLog);
            }
            else
            {
                // Insertar nuevo registro
                await _hangfireRepository.AddAsync(hangfireLog);
                await _hangfireRepository.SaveChangesAsync();
            }

            var kafkaLog = new KafkaLog
            {
                LogType = LogType.Hangfire,
                Topic = message.Topic,
                Message = message.Value,
                Status = "Success",
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(kafkaLog);
            await _kafkaLogRepository.SaveChangesAsync();

            _logger.LogInformation("Hangfire job procesado: {CorrelationId} - {Endpoint}", 
                hangfireLog.CorrelationId, hangfireLog.Endpoint);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando Hangfire log desde tópico {Topic}", message.Topic);
            
            var failureLog = new KafkaLog
            {
                LogType = LogType.Hangfire,
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
