using System.Text.Json.Serialization;
namespace SERVERHANGFIRE.Flows.DTOs
{
    public class EmailTaskDto
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
         [JsonRequired]
        public int CustomerId { get; set; }
    }
}