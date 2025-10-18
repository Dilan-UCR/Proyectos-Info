namespace SERVERHANGFIRE.Flows.Services.Interfaces
{
    public interface IEmailJobService
    {
        Task SendEmailAsync(string correlationId, string toEmail, string subject, string message, int customerId);
    }
}