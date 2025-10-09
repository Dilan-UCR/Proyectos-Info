using PDF_Server.Flows.Services.Interfaces;

namespace PDF_Server.Flows.Services
{
    public class StorageService : IStorageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _storageUrl;

        public StorageService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _storageUrl = configuration["StorageServer:BaseUrl"];
        }

        public async Task<bool> UploadPdfAsync(byte[] pdfBytes, string fileName, string correlationId, int customerId, DateTime dateGeneration)
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(pdfBytes);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            content.Add(fileContent, "file", fileName);
            content.Add(new StringContent(correlationId), "correlationId");
            content.Add(new StringContent(customerId.ToString()), "clientId");
            content.Add(new StringContent(dateGeneration.ToString("yyyy-MM-dd")), "dateGeneration");
            var reponse = await _httpClient.PostAsync(_storageUrl, content);
            return reponse.IsSuccessStatusCode;
        }
    }
}
