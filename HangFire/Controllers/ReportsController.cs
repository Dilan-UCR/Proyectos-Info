using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SERVERHANGFIRE.Flows.DTOs;
using SERVERHANGFIRE.Flows.Services.Interfaces;
using SERVERHANGFIRE.Flows.Validation;

namespace SERVERHANGFIRE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IBackgroundJobClient _hangfire;

        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            IBackgroundJobClient hangfire,
            ILogger<ReportsController> logger)
        {
            _hangfire = hangfire;
            _logger = logger;
        }

        [HttpPost]
       public IActionResult CreateReport([FromBody] PdfRequestDto request)
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
       public IActionResult ScheduleEmailTask([FromBody] EmailTaskDto emailTask)
        {
            try
            {
                _logger.LogInformation("Recibida solicitud de programación de email. CorrelationId: {CorrelationId}", emailTask.CorrelationId);

                var jobId = _hangfire.Schedule<IEmailJobService>(
                    job => job.SendEmailAsync(
                        emailTask.CorrelationId,
                        emailTask.ToEmail,
                        emailTask.Subject,
                        emailTask.Message,
                        emailTask.CustomerId
                    ),
                    TimeSpan.FromMinutes(1)
                );

                _logger.LogInformation("Tarea de email programada exitosamente. JobId: {JobId}, CorrelationId: {CorrelationId}", jobId, emailTask.CorrelationId);

                return Ok(new
                {
                    JobId = jobId,
                    CorrelationId = emailTask.CorrelationId,
                    Status = "Scheduled",
                    ScheduledTime = DateTime.UtcNow.AddMinutes(1),
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
public IActionResult ScheduleMessagingTask([FromBody] MessagingTaskDto messagingTask)
{
    try
    {
        // Validar datos de entrada
        if (string.IsNullOrWhiteSpace(messagingTask.CorrelationId) ||
            string.IsNullOrWhiteSpace(messagingTask.PhoneNumber) ||
            string.IsNullOrWhiteSpace(messagingTask.Message))
        {
            return BadRequest(new { Error = "CorrelationId, PhoneNumber y Message son requeridos" });
        }

        _logger.LogInformation(
            "Solicitud de messaging recibida - CorrelationId: '{CorrelationId}', PhoneNumber: '{PhoneNumber}', Message: '{Message}', CustomerId: {CustomerId}",
            messagingTask.CorrelationId,
            messagingTask.PhoneNumber,
            messagingTask.Message,
            messagingTask.CustomerId);

        var jobId = _hangfire.Schedule<IMessagingJobService>(
            job => job.SendMessageAsync(
                messagingTask.CorrelationId,
                messagingTask.PhoneNumber,  
                "telegram",  
                messagingTask.Message
            ),
            TimeSpan.FromMinutes(1)
        );

        _logger.LogInformation("Tarea de messaging programada. JobId: {JobId}, CorrelationId: {CorrelationId}", jobId, messagingTask.CorrelationId);

        return Ok(new
        {
            JobId = jobId,
            CorrelationId = messagingTask.CorrelationId,
            Status = "Scheduled",
            ScheduledTime = DateTime.UtcNow.AddMinutes(1),
            Message = "Messaging task scheduled successfully",
            PhoneNumber = messagingTask.PhoneNumber
        });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al programar tarea de messaging. CorrelationId: {CorrelationId}", messagingTask?.CorrelationId);
        return StatusCode(500, new { Error = "Error al programar tarea de messaging", Details = ex.Message });
    }
}
    }
}
