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

        public async Task SendMessageAsync(string correlationId, string chatId,  string  platform, string message)
        {
            try
            {
                _logger.LogInformation("Iniciando envío de mensaje. CorrelationId: {CorrelationId}, chatId: {chat}", correlationId, chatId);

                // Aquí harías la llamada al Messaging Server (Node.js)
                // Por ahora solo simulamos
                var messagingPayload = new
                {
                    CorrelationId = correlationId,
                    ChatId = chatId,
                    Plaform = platform,
                    Message = message
                };

                _logger.LogInformation("Simulando envío de mensaje: {@MessagingPayload}", messagingPayload);
                
                // Simular delay de procesamiento
                await Task.Delay(2000);
                
                _logger.LogInformation(" Mensaje enviado exitosamente. CorrelationId: {CorrelationId}", correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error enviando mensaje. CorrelationId: {CorrelationId}", correlationId);
                throw; // Re-lanzar para que Hangfire marque el job como fallido
            }
        }
    }
}