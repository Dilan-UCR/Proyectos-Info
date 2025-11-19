namespace KafkaWorkerService.Configuration;

public class KafkaConfiguration
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string GroupId { get; set; } = "kafka-worker-service";
    public OffsetResetStrategy AutoOffsetReset { get; set; } = OffsetResetStrategy.Earliest;
    public int SessionTimeoutMs { get; set; } = 30000;
    public int MaxPollIntervalMs { get; set; } = 300000;
    public int PollTimeoutMs { get; set; } = 1000;
    public bool EnableAutoCommit { get; set; } = false;
    public int MaxRetries { get; set; } = 3;
    public int RetryBackoffMs { get; set; } = 100;
}

public enum OffsetResetStrategy
{
    Earliest,
    Latest,
    None
}
