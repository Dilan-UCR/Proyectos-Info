namespace PDF_Server.Flows.DTOs
{
    public class NotificationDefaults
    {
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;  
        public string EmailSubject { get; set; } = string.Empty;
        public string SmsMessage { get; set; } = string.Empty;
    }
}