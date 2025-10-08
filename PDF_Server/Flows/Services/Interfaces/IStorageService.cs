namespace PDF_Server.Flows.Services.Interfaces
{
    public interface IStorageService
    {
        Task<bool> UploadPdfAsync(byte[] pdfBytes, string fileName, string correlationId, int customerId, DateTime dateGeneration);
    }
}
