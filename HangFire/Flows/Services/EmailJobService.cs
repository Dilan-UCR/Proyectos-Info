using SERVERHANGFIRE.Flows.Services.Interfaces;

namespace SERVERHANGFIRE.Flows.Services
{
    public class EmailJobService : IEmailJobService
    {
        private readonly ILogger<EmailJobService> _logger;
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;

        public EmailJobService(ILogger<EmailJobService> logger, IHttpClientService httpClientService, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientService = httpClientService;
            _configuration = configuration;
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

                var emailApiUrl = _configuration["ApiEndpoints:EmailService"] 
                    ?? throw new InvalidOperationException("EmailService URL no configurada en appsettings.json");
                
                _logger.LogInformation("Enviando email a API: {EmailApiUrl}", emailApiUrl);
                
                var response = await _httpClientService.PostAsync(emailApiUrl, emailPayload);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Email enviado exitosamente. CorrelationId: {CorrelationId}, Response: {Response}", 
                        correlationId, responseContent);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en Email API. Status: {StatusCode}, Error: {Error}, CorrelationId: {CorrelationId}", 
                        response.StatusCode, errorContent, correlationId);
                    throw new HttpRequestException($"Email API returned {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email. CorrelationId: {CorrelationId}", correlationId);
                throw; 
            }
        }
    }
}