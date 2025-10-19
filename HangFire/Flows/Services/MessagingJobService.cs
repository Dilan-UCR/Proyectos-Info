using SERVERHANGFIRE.Flows.Services.Interfaces;

namespace SERVERHANGFIRE.Flows.Services
{
    public class MessagingJobService : IMessagingJobService
    {
        private readonly ILogger<MessagingJobService> _logger;
        private readonly IHttpClientService _httpClientService;

        public MessagingJobService(ILogger<MessagingJobService> logger, IHttpClientService httpClientService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
        }

        public async Task SendMessageAsync(string correlationId, string chatId, string platform, string message)
        {
            try
            {
                _logger.LogInformation(" Iniciando envío de mensaje. CorrelationId: {CorrelationId}, chatId: {chat}", correlationId, chatId);

                var messagingPayload = new
                {
                    CorrelationId = correlationId,
                    ChatId = chatId,
                    Platform = platform, 
                    Message = message
                };

                
                var messagingApiUrl = "http://localhost:8002/api/messaging/send";
                
                _logger.LogInformation(" Enviando mensaje a API: {MessagingApiUrl}, Payload: {@MessagingPayload}", messagingApiUrl, messagingPayload);
                
                var response = await _httpClientService.PostAsync(messagingApiUrl, messagingPayload);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(" Mensaje enviado exitosamente. CorrelationId: {CorrelationId}, Response: {Response}", 
                        correlationId, responseContent);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError(" Error en Messaging API. Status: {StatusCode}, Error: {Error}, CorrelationId: {CorrelationId}", 
                        response.StatusCode, errorContent, correlationId);
                    throw new Exception($"Messaging API returned {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error enviando mensaje. CorrelationId: {CorrelationId}", correlationId);
                throw;
            }
        }
    }
}