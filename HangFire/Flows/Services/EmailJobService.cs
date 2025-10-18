using SERVERHANGFIRE.Flows.Services.Interfaces;

namespace SERVERHANGFIRE.Flows.Services
{
    public class EmailJobService : IEmailJobService
    {
        private readonly ILogger<EmailJobService> _logger;
        private readonly IHttpClientService _httpClientService;

        public EmailJobService(ILogger<EmailJobService> logger, IHttpClientService httpClientService)
        {
            _logger = logger;
            _httpClientService = httpClientService;
        }

        public async Task SendEmailAsync(string correlationId, string toEmail, string subject, string message, int customerId)
        {
            try
            {
                _logger.LogInformation("🚀 Iniciando envío de email. CorrelationId: {CorrelationId}, Email: {Email}", correlationId, toEmail);

                // Aquí harías la llamada al Email Server (Python)
                // Por ahora solo simulamos
                var emailPayload = new
                {
                    CorrelationId = correlationId,
                    ToEmail = toEmail,
                    Subject = subject,
                    Message = message,
                    CustomerId = customerId
                };

                _logger.LogInformation("📧 Simulando envío de email: {@EmailPayload}", emailPayload);
                
                // Simular delay de procesamiento
                await Task.Delay(2000);
                
                _logger.LogInformation("✅ Email enviado exitosamente. CorrelationId: {CorrelationId}", correlationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error enviando email. CorrelationId: {CorrelationId}", correlationId);
                throw; // Re-lanzar para que Hangfire marque el job como fallido
            }
        }
    }
}