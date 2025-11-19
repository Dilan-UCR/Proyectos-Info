using Confluent.Kafka;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Models;
using System.Text.Json;

namespace KafkaWorkerService.Processors;

public class MessageLogProcessor : IMessageProcessor
{
    private readonly IRepository<MessageLog> _messageRepository;
    private readonly IRepository<KafkaLog> _kafkaLogRepository;
    private readonly ILogger<MessageLogProcessor> _logger;

    public string TopicType => "messages-logs";

    public MessageLogProcessor(
        IRepository<MessageLog> messageRepository,
        IRepository<KafkaLog> kafkaLogRepository,
        ILogger<MessageLogProcessor> logger)
    {
        _messageRepository = messageRepository;
        _kafkaLogRepository = kafkaLogRepository;
        _logger = logger;
    }

    public async Task ProcessAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var messageLog = JsonSerializer.Deserialize<MessageLog>(message.Message.Value, options)
                ?? throw new JsonException("No se pudo deserializar el mensaje de Message");

            await _messageRepository.AddAsync(messageLog);
            await _messageRepository.SaveChangesAsync();

            var kafkaLog = new KafkaLog
            {
                LogType = LogType.Messages,
                Topic = message.Topic,
                Message = message.Message.Value,
                Status = "Success",
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(kafkaLog);
            await _kafkaLogRepository.SaveChangesAsync();

            _logger.LogInformation("Mensaje procesado: CorrelationId={CorrelationId}, Platform={Platform}", 
                messageLog.CorrelationId, messageLog.Platform);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando mensaje desde tópico {Topic}", message.Topic);
            
            var failureLog = new KafkaLog
            {
                LogType = LogType.Messages,
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
