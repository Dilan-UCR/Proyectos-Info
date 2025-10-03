namespace PDF_Server.Flows.Services.Interfaces
{
    public interface IPdfGenerator
    {
        Task GenerateCustomerReportsAsync(DTOs.PdfRequestDto request);
    }
}
