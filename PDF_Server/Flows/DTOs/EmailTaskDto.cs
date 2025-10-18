namespace PDF_Server.Flows.DTOs
{
    public class EmailTaskDto
    {
        public String CorrelationId { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string PdfFileName { get; set; } = string.Empty;

    }
}