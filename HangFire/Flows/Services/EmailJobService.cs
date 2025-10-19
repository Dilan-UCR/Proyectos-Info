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
                _logger.LogInformation("Iniciando envío de email. CorrelationId: {CorrelationId}, Email: {Email}", correlationId, toEmail);

                var emailPayload = new
                {
                    CorrelationId = correlationId,
                    ToEmail = toEmail,
                    Subject = subject,
                    Message = message,
                    CustomerId = customerId
                };

                var emailApiUrl = "http://localhost:8001/api/email/send";
                
                _logger.LogInformation("Enviando email a API: {EmailApiUrl}, Payload: {@EmailPayload}", emailApiUrl, emailPayload);
                
                var response = await _httpClientService.PostAsync(emailApiUrl, emailPayload);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(" Email enviado exitosamente. CorrelationId: {CorrelationId}, Response: {Response}", 
                        correlationId, responseContent);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError(" Error en Email API. Status: {StatusCode}, Error: {Error}, CorrelationId: {CorrelationId}", 
                        response.StatusCode, errorContent, correlationId);
                    throw new Exception($"Email API returned {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error enviando email. CorrelationId: {CorrelationId}", correlationId);
                throw; 
            }
        }
    }
}