namespace PDF_Server.Flows.DTOs
{
    public class ProcessLogDto
    {
        public string CorrelationId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}