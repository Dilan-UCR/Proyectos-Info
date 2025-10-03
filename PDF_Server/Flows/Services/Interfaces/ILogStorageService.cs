using PDF_Server.Flows.DTOs;

namespace PDF_Server.Flows.Services.Interfaces
{
    public interface ILogStorageService
    {
        Task SaveLogAsync(ProcessLogDto log);
        Task SaveLogsAsync(IEnumerable<ProcessLogDto> logs);
    }
}