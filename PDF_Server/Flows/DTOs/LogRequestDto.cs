namespace PDF_Server.Flows.DTOs
{
    public class LogRequestDto
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Payload { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}