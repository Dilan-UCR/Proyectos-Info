using SERVERHANGFIRE.Flows.DTOs;

namespace SERVERHANGFIRE.Flows.Services.Interfaces
{
   public interface IHttpClientService
    {
        Task<bool> SendReportRequestAsync(PdfRequestDto request);
        Task<bool> SendLogAsync(LogRequestDto log); 
        Task<HttpResponseMessage> PostAsync<T>(string url, T payload);   
    }
}
