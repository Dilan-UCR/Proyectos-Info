namespace SERVERHANGFIRE.Flows.Services.Interfaces
{
    public interface IMessagingJobService
    {
        Task SendMessageAsync(string correlationId, string chatId,string platform, string message);
    }
}