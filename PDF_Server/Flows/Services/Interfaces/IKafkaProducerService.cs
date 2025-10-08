using PDF_Server.Flows.DTOs;

namespace PDF_Server.Flows.Services.Interfaces
{
    public interface IKafkaProducerService
    {
        Task<bool> SendLogAsync(LogRequestDto log);
    }
}
