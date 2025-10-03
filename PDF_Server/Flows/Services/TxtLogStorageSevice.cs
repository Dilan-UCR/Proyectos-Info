using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;
using System.Text;

namespace PDF_Server.Flows.Services
{
    public class TxtLogStorageService : ILogStorageService
    {
        private readonly string _filePath;

        public TxtLogStorageService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task SaveLogAsync(ProcessLogDto log)
        {
            await SaveLogsAsync(new List<ProcessLogDto> { log });
        }

        public async Task SaveLogsAsync(IEnumerable<ProcessLogDto> logs)
        {
            var lines = logs.Select(log =>
                $"{log.CorrelationId}\t{log.Timestamp:O}\t{log.Message}");

            await File.AppendAllLinesAsync(_filePath, lines, Encoding.UTF8);
        }
    }
}