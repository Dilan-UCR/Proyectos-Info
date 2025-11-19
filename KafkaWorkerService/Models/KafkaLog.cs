namespace KafkaWorkerService.Models;

public enum LogType
{
    Email,
    Hangfire,
    Pdf,
    Storage,
    Messages
}

public class KafkaLog
{
    public int Id { get; set; }
    public LogType LogType { get; set; }
    public string Topic { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Success";
    public string? ErrorDetails { get; set; }
    public long KafkaOffset { get; set; }
    public int KafkaPartition { get; set; }
}
