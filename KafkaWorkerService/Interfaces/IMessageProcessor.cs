using Confluent.Kafka;

namespace KafkaWorkerService.Interfaces;

public interface IMessageProcessor
{

    string TopicType { get; }

    Task ProcessAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken);
}
