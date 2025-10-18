using PDF_Server.Flows.DTOs;

namespace PDF_Server.Flows.Services.Interfaces
{
    public interface IHangfireTaskService
    {
        Task<bool> ScheduleEmailTaskAsync(EmailTaskDto emailTask);
        Task<bool> ScheduleMessagingTaskAsync(MessagingTaskDto messagingTask);
    }
}