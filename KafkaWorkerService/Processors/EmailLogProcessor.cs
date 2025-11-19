using Confluent.Kafka;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Models;
using System.Text.Json;

namespace KafkaWorkerService.Processors;

public class EmailLogProcessor : IMessageProcessor
{
    private readonly IRepository<EmailLog> _emailRepository;
    private readonly IRepository<KafkaLog> _kafkaLogRepository;
    private readonly ILogger<EmailLogProcessor> _logger;

    public string TopicType => "logs-email";

    public EmailLogProcessor(
        IRepository<EmailLog> emailRepository,
        IRepository<KafkaLog> kafkaLogRepository,
        ILogger<EmailLogProcessor> logger)
    {
        _emailRepository = emailRepository;
        _kafkaLogRepository = kafkaLogRepository;
        _logger = logger;
    }

    public async Task ProcessAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var emailLog = JsonSerializer.Deserialize<EmailLog>(message.Message.Value, options)
                ?? throw new JsonException("No se pudo deserializar el mensaje de Email");

            await _emailRepository.AddAsync(emailLog);
            await _emailRepository.SaveChangesAsync();

            // Registrar en log general de Kafka
            var kafkaLog = new KafkaLog
            {
                LogType = LogType.Email,
                Topic = message.Topic,
                Message = message.Message.Value,
                Status = "Success",
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(kafkaLog);
            await _kafkaLogRepository.SaveChangesAsync();

            _logger.LogInformation("Email procesado: CorrelationId={CorrelationId}, Destinatario={Recipient}", 
                emailLog.CorrelationId, emailLog.RecipientEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando email log desde tópico {Topic}", message.Topic);
            
            var failureLog = new KafkaLog
            {
                LogType = LogType.Email,
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
