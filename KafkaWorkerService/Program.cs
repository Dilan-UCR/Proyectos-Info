using KafkaWorkerService;
using KafkaWorkerService.Configuration;
using KafkaWorkerService.Data;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Processors;
using KafkaWorkerService.Services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// Configurar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' no encontrada.");

builder.Services.AddDbContext<KafkaDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(3);
    }));

// Configurar Kafka
var kafkaConfig = builder.Configuration.GetSection("Kafka").Get<KafkaConfiguration>()
    ?? throw new InvalidOperationException("Configuración de Kafka no encontrada.");

builder.Services.AddSingleton(kafkaConfig);

// Registrar Repositorio Genérico
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Registrar Procesadores
builder.Services.AddScoped<EmailLogProcessor>();
builder.Services.AddScoped<HangfireLogProcessor>();
builder.Services.AddScoped<PdfLogProcessor>();
builder.Services.AddScoped<StorageLogProcessor>();
builder.Services.AddScoped<MessageLogProcessor>();

// Registrar Consumidor de Kafka y Worker
builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumerService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// Aplicar migraciones automáticamente al iniciar
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<KafkaDbContext>();
    await dbContext.Database.MigrateAsync();
}

host.Run();
  