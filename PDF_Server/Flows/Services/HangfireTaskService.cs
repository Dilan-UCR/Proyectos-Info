using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace PDF_Server.Flows.Services
{
    public class HangfireTaskService : IHangfireTaskService
    {
        private readonly HttpClient _httpClient;
        private readonly string _hangfireBaseUrl;
        private readonly ILogger<HangfireTaskService> _logger;

        public HangfireTaskService(HttpClient httpClient, IConfiguration configuration, ILogger<HangfireTaskService> logger)
        {
            _httpClient = httpClient;
            _hangfireBaseUrl = configuration["HangfireServer:BaseUrl"] ?? throw new ArgumentNullException("HangfireServer:BaseUrl not configured");
            _logger = logger;
        }

        public async Task<bool> ScheduleEmailTaskAsync(EmailTaskDto emailTask)
        {
            try
            {
                var json = JsonSerializer.Serialize(emailTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_hangfireBaseUrl}/schedule-email", content);
             
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Email task scheduled successfully. CorrelationId: {emailTask.CorrelationId}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Failed to schedule email task. Status: {response.StatusCode}. CorrelationId: {emailTask.CorrelationId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error scheduling email task. CorrelationId: {emailTask.CorrelationId}");
                return false;
            }
        }

        public async Task<bool> ScheduleMessagingTaskAsync(MessagingTaskDto messagingTask)
        {
            try
            {
                var json = JsonSerializer.Serialize(messagingTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync($"{_hangfireBaseUrl}/schedule-messaging", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Messaging task scheduled successfully. CorrelationId: {messagingTask.CorrelationId}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Failed to schedule messaging task. Status: {response.StatusCode}. CorrelationId: {messagingTask.CorrelationId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error scheduling messaging task. CorrelationId: {messagingTask.CorrelationId}");
                return false;
            }
        }
    }
}