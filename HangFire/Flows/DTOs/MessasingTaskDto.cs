namespace SERVERHANGFIRE.Flows.DTOs
{
    public class MessagingTaskDto
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int CustomerId { get; set; }
       // public string PdfFileName { get; set; } = string.Empty;
    }
}