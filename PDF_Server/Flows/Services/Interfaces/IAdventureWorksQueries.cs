using PDF_Server.Flows.DTOs;

namespace PDF_Server.Flows.Services.Interfaces
{
    public interface IAdventureWorksQueries
    {
        Task<IEnumerable<Result1>> Query1Async(PdfRequestDto request);
        //Task<IEnumerable<Result2>> Query2Async(PdfRequestDto request);
        //Task<IEnumerable<Result3>> Query3Async(PdfRequestDto request);
        Task<CustomerInfo?> getCostomerAsync(int customerId);
    } 
}
