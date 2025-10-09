using System.Net.Http.Json;
using SERVERHANGFIRE.Flows.DTOs;
using Microsoft.Extensions.Logging;
using SERVERHANGFIRE.Flows.Services.Interfaces;

namespace SERVERHANGFIRE.Flows.Services
{
   

    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<HttpClientService> _logger;
        private readonly PdfServerOptions _pdfOptions;

        public HttpClientService(
            HttpClient httpClient,
            IKafkaProducerService kafkaProducer,
            ILogger<HttpClientService> logger,
            Microsoft.Extensions.Options.IOptions<PdfServerOptions> pdfOptions)
        {
            _httpClient = httpClient;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _pdfOptions = pdfOptions.Value;
        }

        public async Task<bool> SendReportRequestAsync(PdfRequestDto request)
        {
            try
            {
                _logger.LogInformation("URL destino PDF_Server: {Url}", _pdfOptions.BaseUrl);
                var response = await _httpClient.PostAsJsonAsync(_pdfOptions.BaseUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ PDF server respondió correctamente. CorrelationId={CorrelationId}", request.CorrelationId);
                    return true;
                }
                else
                {
                    _logger.LogError("❌ PDF server devolvió error {StatusCode}. CorrelationId={CorrelationId}", 
                        response.StatusCode, request.CorrelationId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al conectar con el PDF server. CorrelationId={CorrelationId}", request.CorrelationId);
                return false;
            }
        }

        public async Task<bool> SendLogAsync(LogRequestDto log)
        {
            return await _kafkaProducer.SendLogAsync(log);
        }
    }
}