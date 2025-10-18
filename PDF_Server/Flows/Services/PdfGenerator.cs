using PDF_Server.Flows.DTOs;
using PDF_Server.Flows.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType; 

namespace PDF_Server.Flows.Services
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IAdventureWorksQueries _queries;
        public PdfGenerator(IAdventureWorksQueries queries)
        {
            _queries = queries;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateCustomerReportsAsync(PdfRequestDto request)
        {
            var culture = new CultureInfo("es-CR");
            var data1 = await _queries.Query1Async(request);
            //var data2 = await _queries.Query2Async(request);
            //var data3 = await _queries.Query3Async(request);

            var customer = await _queries.getCostomerAsync(request.CustomerId);

            return GeneratePdf(data1, "Reporte 1", customer);
            //GeneratePdf("Report2.pdf", data2, "Reporte 2");
            //GeneratePdf("Report3.pdf", data3, "Reporte 3");

            
        }
        private byte[] GeneratePdf<T>(IEnumerable<T> data, string title, CustomerInfo customer)
        {
            var props = typeof(T).GetProperties();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(TextStyle.Default.FontSize(12));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("FACTURA ORDENES DE VENTAS").FontSize(20).SemiBold();
                            col.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy}");
                            col.Item().Text("Cliente: " + customer.FirstName + " " + customer.LastName);
                        });

                        row.ConstantItem(200).AlignRight().Column(col =>
                        {
                            col.Item().AlignRight().Text("Proyecto Informatica").SemiBold();
                            col.Item().AlignRight().Text("Universidad de Costa Rica");
                        });
                    });
                    page.Content().Column(col =>
                    {
                        col.Spacing(8);
                        col.Item().PaddingVertical(6).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var _ in props)
                                    columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                for (int i = 0; i < props.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        header.Cell().Background(Colors.Grey.Lighten3).AlignLeft().Text(props[i].Name).SemiBold();
                                    }
                                    else
                                    {
                                        header.Cell().Background(Colors.Grey.Lighten3).AlignRight().Text(props[i].Name).SemiBold();
                                    }
                                }
                                
                            });

                            var totalAmount = 0.0m;
                            foreach (var item in data)
                            {
                                table.Cell().PaddingVertical(4).AlignLeft().Text(props[0].GetValue(item)?.ToString() ?? "");
                                for (int i = 1; i < props.Length; i++)
                                {
                                    table.Cell().PaddingVertical(4).AlignRight().Text(props[i].GetValue(item)?.ToString() ?? "");
                                }
                                totalAmount += Convert.ToDecimal(props[2].GetValue(item) ?? 0);
                            }
                            table.Cell().ColumnSpan(3).PaddingTop(4).Element(e => e.LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2));
                            table.Cell().ColumnSpan(2).AlignRight().PaddingTop(6).Text("Total:").SemiBold();
                            table.Cell().AlignRight().PaddingTop(6).Text(totalAmount.ToString()).SemiBold();
                        });
                    });
                    
                });
            });

            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
