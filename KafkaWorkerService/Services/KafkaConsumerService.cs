using Confluent.Kafka;
using KafkaWorkerService.Configuration;
using KafkaWorkerService.Interfaces;
using KafkaWorkerService.Processors;
using Polly;

namespace KafkaWorkerService.Services;

public class KafkaConsumerService : IKafkaConsumer
{
    private readonly KafkaConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly IAsyncPolicy<bool> _retryPolicy;
    private IConsumer<string, string>? _consumer;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _consumerTask;
    private readonly Dictionary<string, Type> _processorTypeMap;

    public KafkaConsumerService(
        KafkaConfiguration config,
        IServiceProvider serviceProvider,
        ILogger<KafkaConsumerService> logger)
    {
        _config = config;
        _serviceProvider = serviceProvider;
        _logger = logger;
        
        _processorTypeMap = new Dictionary<string, Type>
        {
            { "logs-email", typeof(EmailLogProcessor) },
            { "logs-hangfire", typeof(HangfireLogProcessor) },
            { "logs-pdf", typeof(PdfLogProcessor) },
            { "logs-storage", typeof(StorageLogProcessor) },
            { "messages-logs", typeof(MessageLogProcessor) }
        };

        // Configurar política de reintentos con Polly (sin circuit breaker para simplificar)
        _retryPolicy = Policy
            .Handle<KafkaException>()
            .OrResult<bool>(r => !r)
            .WaitAndRetryAsync(
                retryCount: _config.MaxRetries,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(
                    _config.RetryBackoffMs * (int)Math.Pow(2, attempt - 1)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "Reintentando procesamiento. Intento {RetryCount}/{MaxRetries} después de {Delay}ms",
                        retryCount, _config.MaxRetries, timespan.TotalMilliseconds);
                });
    }

    public async Task StartAsync(IEnumerable<string> topics, CancellationToken cancellationToken)
    {
        try
        {
            var topicList = topics.ToList();
            _logger.LogInformation("Iniciando consumidor Kafka para tópicos: {Topics}",
                string.Join(", ", topicList));

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                GroupId = _config.GroupId,
                AutoOffsetReset = MapAutoOffsetReset(_config.AutoOffsetReset),
                SessionTimeoutMs = _config.SessionTimeoutMs,
                MaxPollIntervalMs = _config.MaxPollIntervalMs,
                EnableAutoCommit = _config.EnableAutoCommit,
                StatisticsIntervalMs = 5000
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig)
                .SetLogHandler((_, logMessage) =>
                {
                    _logger.LogInformation("[{Facility}] {Message}", logMessage.Name, logMessage.Message);
                })
                .SetErrorHandler((_, error) =>
                {
                    _logger.LogError("Error en Kafka: {Code} - {Reason}", error.Code, error.Reason);
                })
                .SetStatisticsHandler((_, json) =>
                {
                    _logger.LogDebug("Estadísticas de Kafka: {Json}", json);
                })
                .Build();

            _consumer.Subscribe(topicList);

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _consumerTask = ConsumeMessagesAsync(_cancellationTokenSource.Token);

            await _consumerTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error iniciando consumidor Kafka");
            throw;
        }
    }

    public async Task StopAsync()
    {
        _logger.LogInformation("Deteniendo consumidor Kafka...");
        _cancellationTokenSource?.Cancel();

        if (_consumerTask != null)
        {
            try
            {
                await _consumerTask;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumidor Kafka detenido");
            }
        }

        _consumer?.Close();
    }

    private async Task ConsumeMessagesAsync(CancellationToken cancellationToken)
    {
        if (_consumer == null)
            throw new InvalidOperationException("Consumidor no inicializado");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = _consumer.Consume(_config.PollTimeoutMs);

                    if (message == null)
                        continue;

                    await ProcessMessageWithRetryAsync(message, cancellationToken);

                    // Commit manual después del procesamiento exitoso
                    if (!_config.EnableAutoCommit)
                    {
                        _consumer.Commit(message);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error consumiendo mensaje de Kafka");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fatal en el loop de consumo");
            throw;
        }
    }

    private async Task ProcessMessageWithRetryAsync(ConsumeResult<string, string> message, CancellationToken cancellationToken)
    {
        var topic = message.Topic;

        if (!_processorTypeMap.TryGetValue(topic, out var processorType))
        {
            _logger.LogWarning("No hay procesador registrado para el tópico: {Topic}", topic);
            return;
        }

        await _retryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                _logger.LogInformation(
                    "Procesando mensaje del tópico {Topic}, partición {Partition}, offset {Offset}",
                    topic, message.Partition.Value, message.Offset.Value);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var processor = scope.ServiceProvider.GetService(processorType) as IMessageProcessor;
                    if (processor == null)
                    {
                        _logger.LogError("No se pudo resolver el procesador para el tópico {Topic}", topic);
                        return false;
                    }

                    await processor.ProcessAsync(message, cancellationToken);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando mensaje del tópico {Topic}", topic);
                return false;
            }
        });
    }

    private global::Confluent.Kafka.AutoOffsetReset MapAutoOffsetReset(OffsetResetStrategy config) => config switch
    {
        OffsetResetStrategy.Earliest => global::Confluent.Kafka.AutoOffsetReset.Earliest,
        OffsetResetStrategy.Latest => global::Confluent.Kafka.AutoOffsetReset.Latest,
        OffsetResetStrategy.None => global::Confluent.Kafka.AutoOffsetReset.Earliest,
        _ => global::Confluent.Kafka.AutoOffsetReset.Earliest
    };

    public void Dispose()
    {
        _consumer?.Dispose();
        _cancellationTokenSource?.Dispose();
        GC.SuppressFinalize(this);
    }
}
