using Microsoft.Data.SqlClient;
using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;
using Dapper;

namespace PDF_Server.Flows.Services 
{
    public class AdventureWorksQueries : IAdventureWorksQueries
    {
        private readonly string _connectionString;
        public AdventureWorksQueries(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AdventureWorks");
        }

        public async Task<string?> getCostomerAsync(int customerId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"SELECT FirstName, LastName 
                FROM Sales.Customer AS SC inner join Person.Person AS PP ON SC.PersonID = PP.BusinessEntityID 
                WHERE SC.CustomerID = @CustomerId";
            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { CustomerId = customerId });
        }

        public async Task<IEnumerable<Result1>> Query1Async(PdfRequestDto request)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"SELECT SalesOrderID, OrderDate, TotalDue
               FROM Sales.SalesOrderHeader
               WHERE CustomerID = @CustomerId AND OrderDate BETWEEN @StartDate AND @EndDate";
            return await connection.QueryAsync<Result1>(sql, new { request.CustomerId, request.StartDate, request.EndDate });
        }

        async Task<CustomerInfo?> IAdventureWorksQueries.getCostomerAsync(int customerId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"SELECT FirstName, LastName 
                FROM Sales.Customer AS SC inner join Person.Person AS PP ON SC.PersonID = PP.BusinessEntityID 
                WHERE SC.CustomerID = @CustomerId";
            return await connection.QueryFirstOrDefaultAsync<CustomerInfo>(sql, new { CustomerId = customerId });
        }





        /*public Task<IEnumerable<Result2>> Query2Async(PdfRequestDto request)
        {
            // Implementación aquí
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Result3>> Query3Async(PdfRequestDto request)
        {
            // Implementación aquí
            throw new NotImplementedException();
        }*/
    }
}
