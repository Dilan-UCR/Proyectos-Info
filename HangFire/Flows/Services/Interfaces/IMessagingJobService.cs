namespace SERVERHANGFIRE.Flows.Services.Interfaces
{
    public interface IMessagingJobService
    {
        Task SendMessageAsync(string correlationId, string phoneNumber, string message, int customerId);
    }
}