namespace SERVERHANGFIRE.Flows.Services.Interfaces
{
     public interface IReportJobService
    {
        Task ProcessReportRequest(int customerId, DateTime startDate, DateTime endDate, string correlationId ,List<int> products);
    }

}
 