using Confluent.Kafka;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Models;
using System.Text.Json;

namespace KafkaWorkerService.Processors;

public class PdfLogProcessor : IMessageProcessor
{
    private readonly IRepository<PdfLog> _pdfRepository;
    private readonly IRepository<KafkaLog> _kafkaLogRepository;
    private readonly ILogger<PdfLogProcessor> _logger;

    public string TopicType => "logs-pdf";

    public PdfLogProcessor(
        IRepository<PdfLog> pdfRepository,
        IRepository<KafkaLog> kafkaLogRepository,
        ILogger<PdfLogProcessor> logger)
    {
        _pdfRepository = pdfRepository;
        _kafkaLogRepository = kafkaLogRepository;
        _logger = logger;
    }

    public async Task ProcessAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken)
    {
        try
        {
            var pdfLog = JsonSerializer.Deserialize<PdfLog>(message.Value)
                ?? throw new JsonException("No se pudo deserializar el mensaje de PDF");

            // Patrón UPSERT: Buscar por CorrelationId
            var existingPdf = await _pdfRepository.GetByPredicateAsync(p => p.CorrelationId == pdfLog.CorrelationId);
            
            if (existingPdf != null)
            {
                // Actualizar registro existente
                existingPdf.Service = pdfLog.Service;
                existingPdf.Endpoint = pdfLog.Endpoint;
                existingPdf.Timestamp = pdfLog.Timestamp;
                existingPdf.Payload = pdfLog.Payload;
                existingPdf.Success = pdfLog.Success;
                await _pdfRepository.UpdateAsync(existingPdf);
            }
            else
            {
                // Crear nuevo registro
                await _pdfRepository.AddAsync(pdfLog);
            }

            await _pdfRepository.SaveChangesAsync();

            var kafkaLog = new KafkaLog
            {
                LogType = LogType.Pdf,
                Topic = message.Topic,
                Message = message.Value,
                Status = "Success",
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(kafkaLog);
            await _kafkaLogRepository.SaveChangesAsync();

            _logger.LogInformation("PDF procesado: CorrelationId={CorrelationId}, Éxito={Success}", pdfLog.CorrelationId, pdfLog.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando PDF log desde tópico {Topic}", message.Topic);
            
            var failureLog = new KafkaLog
            {
                LogType = LogType.Pdf,
                Topic = message.Topic,
                Message = message.Value,
                Status = "Failed",
                ErrorDetails = ex.Message,
                KafkaOffset = message.Offset.Value,
                KafkaPartition = message.Partition.Value
            };

            await _kafkaLogRepository.AddAsync(failureLog);
            await _kafkaLogRepository.SaveChangesAsync();

            throw;
        }
    }
}
