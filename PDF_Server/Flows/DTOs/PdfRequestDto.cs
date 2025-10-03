namespace PDF_Server.Flows.DTOs
{
    public class PdfRequestDto
    {
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CorrelationId { get; set; } = string.Empty;
    }
}
