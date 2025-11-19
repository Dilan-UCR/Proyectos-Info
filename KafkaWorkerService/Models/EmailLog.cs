namespace KafkaWorkerService.Models;

public class EmailLog
{
    public int Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
