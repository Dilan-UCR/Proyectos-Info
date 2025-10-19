using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;
using System.Text.Json;

namespace PDF_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IStorageService _storageService;
        private readonly IKafkaProducerService _kafkaProducerService;
        private readonly IHangfireTaskService _hangfireTaskService;
        private readonly NotificationDefaults _notificationDefaults;

        public PdfController(IPdfGenerator pdfGenerator, IStorageService storageService, IKafkaProducerService kafkaProducerService, IHangfireTaskService hangfireTaskService, IOptions<NotificationDefaults> notificationDefaults)
        {
            _pdfGenerator = pdfGenerator;
            _storageService = storageService;
            _kafkaProducerService = kafkaProducerService;
            _hangfireTaskService = hangfireTaskService;
            _notificationDefaults = notificationDefaults.Value;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePdf([FromBody] PdfRequestDto request, CancellationToken cancellationToken)
        {
            await LogAsync(request.CorrelationId, request, "Datos recibidos correctamente", cancellationToken);
            try
            {
                byte[] pdfBytes = await _pdfGenerator.GenerateCustomerReportsAsync(request);
                
                DateTime dateGeneration = DateTime.UtcNow;
                string nameFile = $"Bill_{request.CustomerId}_{dateGeneration:yyyyMMdd_HHmmss}.pdf";
 
                bool storageSuccess = false;
                
                try
                {
                    storageSuccess = await _storageService.UploadPdfAsync(
                        pdfBytes,
                        nameFile,
                        request.CorrelationId,
                        request.CustomerId,
                        dateGeneration
                    );
                    Console.WriteLine($" Resultado almacenamiento: {storageSuccess}");
                }
                catch (Exception storageEx)
                {
                    Console.WriteLine($" Error en Storage Server: {storageEx.Message}");
                    storageSuccess = false;
                }

               //Solo programar tareas si el almacenamiento fue exitoso
                if (storageSuccess)
                {
                    await ScheduleNotificationTasksAsync(request, nameFile);
                    
                    await LogAsync(request.CorrelationId, request, "PDF generado, almacenado y tareas programadas exitosamente", cancellationToken);
                    return Ok(new { 
                        Message = "PDF generado correctamente", 
                        StorageSuccess = true,
                        TasksScheduled = true 
                    });
                }
                else
                {
                    Console.WriteLine("Error al almacenar el PDF en Storage Server- NO SE PROGRAMAN TAREAS");
                    await LogAsync(request.CorrelationId, request, "PDF generado pero fallo en almacenamiento - No se programaron tareas", cancellationToken);
                    return StatusCode(500, new { 
                        Message = "Error al almacenar el PDF en Storage Server", 
                        StorageSuccess = false,
                        TasksScheduled = false 
                    });
                }
            }
            catch (Exception ex)
            {
                await LogAsync(request.CorrelationId, request, $"Error al consultar la base de datos o crear el PDF: {ex.Message}", cancellationToken);
                return StatusCode(500, new { Message = "Error al consultar la base de datos o crear el PDF" });
            }
        }

        private async Task ScheduleNotificationTasksAsync(PdfRequestDto request, string fileName)
        {
            try
            {

                var emailTask = new EmailTaskDto
                {
                    CorrelationId = request.CorrelationId,
                    ToEmail = _notificationDefaults.Email,
                    Subject = _notificationDefaults.EmailSubject,
                    Message = $"{_notificationDefaults.SmsMessage} Cliente: {request.CustomerId}, Archivo: {fileName}",
                    CustomerId = request.CustomerId
                };
             
                bool emailResult = await _hangfireTaskService.ScheduleEmailTaskAsync(emailTask);
                Console.WriteLine($"Tarea EMAIL programada: {emailResult}");
           
                var messagingTask = new MessagingTaskDto
                {
                    CorrelationId = request.CorrelationId,
                    PhoneNumber = _notificationDefaults.PhoneNumber,
                    Message = $"{_notificationDefaults.SmsMessage} Cliente: {request.CustomerId}, Archivo: {fileName}",
                    CustomerId = request.CustomerId
                };
                bool messagingResult = await _hangfireTaskService.ScheduleMessagingTaskAsync(messagingTask);
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ERROR al programar tareas: {ex.Message}");
                await LogAsync(request.CorrelationId, request, $"Error al programar tareas de notificación: {ex.Message}", CancellationToken.None);
            }
        }

        private async Task LogAsync(string correlationId, PdfRequestDto request, string message, CancellationToken cancellationToken)
        {
            var payload = new
            {
                Request = request,
                Message = message
            };
            LogRequestDto logRequest = new LogRequestDto
            {
                CorrelationId = correlationId,
                Service = "PDF Server",
                Endpoint = HttpContext?.Request?.Path.Value ?? string.Empty,
                Timestamp = DateTime.UtcNow,
                Payload = JsonSerializer.Serialize(payload),
                Success = !message.ToLower().Contains("error")
            };
            await _kafkaProducerService.SendLogAsync(logRequest);
        }
    }
}