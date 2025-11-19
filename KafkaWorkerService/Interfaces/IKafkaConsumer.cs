using Confluent.Kafka;

namespace KafkaWorkerService.Interfaces;

public interface IKafkaConsumer : IDisposable
{
    Task StartAsync(IEnumerable<string> topics, CancellationToken cancellationToken);

    Task StopAsync();
}
