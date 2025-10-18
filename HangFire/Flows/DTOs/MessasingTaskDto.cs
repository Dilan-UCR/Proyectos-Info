namespace SERVERHANGFIRE.Flows.DTOs
{
    public class MessagingTaskDto
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string ChatId { get; set; } = string.Empty;
          public string Platform { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        
    }
}