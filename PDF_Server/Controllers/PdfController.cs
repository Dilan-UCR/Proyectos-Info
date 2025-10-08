using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public PdfController(IPdfGenerator pdfGenerator, IStorageService storageService, IKafkaProducerService kafkaProducerService)
        {
            _pdfGenerator = pdfGenerator;
            _storageService = storageService;
            _kafkaProducerService = kafkaProducerService;
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
                await _storageService.UploadPdfAsync(
                    pdfBytes,
                    nameFile,
                    request.CorrelationId,
                    request.CustomerId,
                    dateGeneration
                );
                await LogAsync(request.CorrelationId, request, "Consulta a la base de datos y creacion de PDF exitosa", cancellationToken);
                return Ok(new { Message = "PDF generado correctamente" });
            }
            catch (Exception ex)
            {
                await LogAsync(request.CorrelationId, request, $"Error al consultar la base de datos o crear el PDF: {ex.Message}", cancellationToken);
                return StatusCode(500, new { Message = "Error al consultar la base de datos o crear el PDF" });
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
