using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;

namespace PDF_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly ILogStorageService _logStorageService;
        private readonly IStorageService _storageService;

        public PdfController(IPdfGenerator pdfGenerator, ILogStorageService logStorageService, IStorageService storageService)
        {
            _pdfGenerator = pdfGenerator;
            _logStorageService = logStorageService;
            _storageService = storageService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePdf([FromBody] PdfRequestDto request, CancellationToken cancellationToken)
        {
            await LogAsync(request.CorrelationId, "Datos recibidos correctamente", cancellationToken);
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
                await LogAsync(request.CorrelationId, "Consulta a la base de datos y creación de PDF exitosa", cancellationToken);
                return Ok(new { Message = "PDF generado correctamente" });
            }
            catch (Exception ex)
            {
                await LogAsync(request.CorrelationId, $"Error al consultar la base de datos o crear el PDF: {ex.Message}", cancellationToken);
                return StatusCode(500, new { Message = "Error al consultar la base de datos o crear el PDF" });
            }
        }

        private async Task LogAsync(string correlationId, string message, CancellationToken cancellationToken)
        {
            ProcessLogDto log = new ProcessLogDto
            {
                CorrelationId = correlationId,
                Timestamp = DateTime.UtcNow,
                Message = message
            };
            await _logStorageService.SaveLogAsync(log);
        }
    }
}
