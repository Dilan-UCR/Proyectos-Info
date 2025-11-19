namespace KafkaWorkerService.Models;

public class StorageLog
{
    public int Id { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? Payload { get; set; }
    public bool Success { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
