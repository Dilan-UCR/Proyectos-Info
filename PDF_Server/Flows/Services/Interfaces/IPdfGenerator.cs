namespace PDF_Server.Flows.Services.Interfaces
{
    public interface IPdfGenerator
    {
        Task<byte[]> GenerateCustomerReportsAsync(DTOs.PdfRequestDto request);
    }
}
 