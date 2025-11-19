namespace KafkaWorkerService.Models;

public class MessageLog
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string ChatId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
