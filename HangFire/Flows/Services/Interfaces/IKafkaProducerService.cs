using SERVERHANGFIRE.Flows.DTOs;

namespace SERVERHANGFIRE.Flows.Services.Interfaces
{
       public interface IKafkaProducerService
    {
        Task<bool> SendLogAsync(LogRequestDto log);
    }
}
 