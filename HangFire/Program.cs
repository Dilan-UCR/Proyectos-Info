using Hangfire;
using Hangfire.SqlServer;
using SERVERHANGFIRE.Flows.Services;
using SERVERHANGFIRE.Flows.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection("Kafka"));


builder.Services.AddScoped<IReportJobService, ReportJobService>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>(); 
builder.Services.AddScoped<IEmailJobService, EmailJobService>();       
builder.Services.AddScoped<IMessagingJobService, MessagingJobService>(); 
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.Configure<PdfServerOptions>(builder.Configuration.GetSection("PdfServer"));

builder.Services.AddHttpClient<IHttpClientService, HttpClientService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


builder.Services.AddHangfire(config =>
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(
              builder.Configuration.GetConnectionString("AdventureWorks"),
              new SqlServerStorageOptions
              {
                  CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                  SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                  QueuePollInterval = TimeSpan.Zero,
                  UseRecommendedIsolationLevel = true,
                  DisableGlobalLocks = true
              }
          )
);

builder.Services.AddHangfireServer();

var app = builder.Build();


app.UseRouting();
app.UseAuthorization();


app.MapControllers();
app.UseHangfireDashboard("/hangfire");

await app.RunAsync();
