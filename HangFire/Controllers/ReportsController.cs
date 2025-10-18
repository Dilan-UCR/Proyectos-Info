using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SERVERHANGFIRE.Flows.DTOs;
using SERVERHANGFIRE.Flows.Services;
using SERVERHANGFIRE.Flows.Services.Interfaces;
using SERVERHANGFIRE.Flows.Validation;

namespace SERVERHANGFIRE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IBackgroundJobClient _hangfire;
        private readonly IHttpClientService _httpClientService;
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            IBackgroundJobClient hangfire,
            IKafkaProducerService kafkaProducer,
            ILogger<ReportsController> logger,
            IHttpClientService httpClientService)
        {
            _hangfire = hangfire;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
            _httpClientService = httpClientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] PdfRequestDto request)
        {
            if (!ReportRequestValidator.IsValid(request, out string errorMessage))
            {
                return BadRequest(new { Error = errorMessage });
            }

            string correlationId = string.IsNullOrWhiteSpace(request.CorrelationId)
                ? Guid.NewGuid().ToString()
                : request.CorrelationId;

            try
            {
                _hangfire.Schedule<IReportJobService>(
                    job => job.ProcessReportRequest(
                        request.CustomerId,
                        request.StartDate,
                        request.EndDate,
                        correlationId,
                        request.Products
                    ),
                    TimeSpan.FromMinutes(1) 
                );
                
                _logger.LogInformation("Solicitud encolada. CorrelationId={CorrelationId}", correlationId);

                return Ok(new
                {
                    CorrelationId = correlationId,
                    Status = "Scheduled",
                    ScheduledTime = DateTime.UtcNow.AddMinutes(5),
                    Message = "La generación del reporte comenzará en 5 minutos"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al encolar tarea. CorrelationId={CorrelationId}", correlationId);
                return StatusCode(500, new { Error = "Error interno del servidor" });
            }
        }

        [HttpPost("schedule-email")]
        public async Task<IActionResult> ScheduleEmailTask([FromBody] EmailTaskDto emailTask)
        {
            try
            {
                _logger.LogInformation("📧 Recibida solicitud de programación de email. CorrelationId: {CorrelationId}", emailTask.CorrelationId);
                
                // Programar tarea de email con delay de 30 segundos
                var jobId = _hangfire.Schedule<IEmailJobService>(
                    job => job.SendEmailAsync(
                        emailTask.CorrelationId,
                        emailTask.ToEmail,
                        emailTask.Subject,
                        emailTask.Message,
                        emailTask.CustomerId
                    ),
                    TimeSpan.FromSeconds(45) 
                );

                _logger.LogInformation("✅ Tarea de email programada exitosamente. JobId: {JobId}, CorrelationId: {CorrelationId}", jobId, emailTask.CorrelationId);

                return Ok(new
                {
                    JobId = jobId,
                    CorrelationId = emailTask.CorrelationId,
                    Status = "Scheduled",
                    ScheduledTime = DateTime.UtcNow.AddSeconds(30),
                    Message = "Email task scheduled successfully",
                    EmailTo = emailTask.ToEmail
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al programar tarea de email. CorrelationId: {CorrelationId}", emailTask.CorrelationId);
                return StatusCode(500, new { Error = "Error al programar tarea de email", Details = ex.Message });
            }
        }

        [HttpPost("schedule-messaging")]
        public async Task<IActionResult> ScheduleMessagingTask([FromBody] MessagingTaskDto messagingTask)
        {
            try
            {
                _logger.LogInformation("Recibida solicitud de programación de messaging. CorrelationId: {CorrelationId}", messagingTask.CorrelationId);
                
                // Programar tarea de messaging con delay de 45 segundos
                var jobId = _hangfire.Schedule<IMessagingJobService>(
                    job => job.SendMessageAsync(
                        messagingTask.CorrelationId,
                        messagingTask.PhoneNumber,
                        messagingTask.Message,
                        messagingTask.CustomerId
                    ),
                    TimeSpan.FromSeconds(45) 
                );

                _logger.LogInformation("Tarea de messaging programada exitosamente. JobId: {JobId}, CorrelationId: {CorrelationId}", jobId, messagingTask.CorrelationId);

                return Ok(new
                {
                    JobId = jobId,
                    CorrelationId = messagingTask.CorrelationId,
                    Status = "Scheduled",
                    ScheduledTime = DateTime.UtcNow.AddSeconds(45),
                    Message = "Messaging task scheduled successfully",
                    PhoneNumber = messagingTask.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al programar tarea de messaging. CorrelationId: {CorrelationId}", messagingTask.CorrelationId);
                return StatusCode(500, new { Error = "Error al programar tarea de messaging", Details = ex.Message });
            }
        }
    }
}