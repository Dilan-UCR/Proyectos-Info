using Confluent.Kafka;
using PDF_Server.Flows.Services;
using PDF_Server.Flows.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAdventureWorksQueries, AdventureWorksQueries>();
builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();

builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = sp.GetRequiredService<IConfiguration>()["Kafka:BootstrapServers"]
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

builder.Services.AddSingleton<ILogStorageService>(sp =>
{
    var filePath = sp.GetRequiredService<IConfiguration>()["LogStorage:FilePath"];
    return new TxtLogStorageService(filePath);
});

builder.Services.AddHostedService<PdfRequestConsumerService>();

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