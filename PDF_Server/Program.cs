using PDF_Server.Flows.Services;
using PDF_Server.Flows.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAdventureWorksQueries, AdventureWorksQueries>();
builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();
builder.Services.AddScoped<ILogStorageService>(provider =>
    new TxtLogStorageService("logs.txt"));
builder.Services.AddScoped<IStorageService, StorageService>();


builder.WebHost.UseUrls("http://+:5000");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();