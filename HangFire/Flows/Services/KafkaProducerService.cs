using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;
using SERVERHANGFIRE.Flows.DTOs;
using SERVERHANGFIRE.Flows.Services.Interfaces;

namespace SERVERHANGFIRE.Flows.Services
{
    public class KafkaOptions
    {
        public string BootstrapServers { get; set; } = "48.211.170.113:9092";
        public string Topic { get; set; } = "logs-hangfire";
    }

    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;
        private readonly ILogger<KafkaProducerService> _logger;
        private bool _disposed;

        public KafkaProducerService(IOptions<KafkaOptions> options, ILogger<KafkaProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.Value.BootstrapServers,
                MessageTimeoutMs = 2000
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = options.Value.Topic;
            _logger = logger;
        }

        public async Task<bool> SendLogAsync(LogRequestDto log)
        {
            try
            {
                var message = JsonSerializer.Serialize(log);

                await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });

                _logger.LogInformation("Log enviado a Kafka. CorrelationId={CorrelationId}", log.CorrelationId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar log a Kafka. CorrelationId={CorrelationId}", log.CorrelationId);
                return false;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _producer?.Flush(TimeSpan.FromSeconds(5));
                    _producer?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}