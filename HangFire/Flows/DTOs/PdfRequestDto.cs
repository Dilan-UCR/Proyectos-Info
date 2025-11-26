using Newtonsoft.Json;

namespace SERVERHANGFIRE.Flows.DTOs
{
    public class LogRequestDto
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;

        [JsonRequired]
        public DateTime Timestamp { get; set; }

        public string Payload { get; set; } = string.Empty;

        [JsonRequired]
        public bool Success { get; set; }
    }

    public class PdfRequestDto
    {
        [JsonRequired]
        public int CustomerId { get; set; }

        [JsonRequired]
        public DateTime StartDate { get; set; }

        [JsonRequired]
        public DateTime EndDate { get; set; }

        public string CorrelationId { get; set; } = string.Empty;

        [JsonRequired]
        public List<int> Products { get; set; } = new List<int>();
    }
}
