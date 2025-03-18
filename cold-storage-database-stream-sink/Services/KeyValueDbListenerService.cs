using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.Runtime;
using cold_storage_database_stream_sink.Models.Contracts;
using cold_storage_database_stream_sink.src.Configs.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace cold_storage_database_stream_sink.src.Services
{
    public class KeyValueDbListenerService : IHostedService
    {
        private readonly ILogger<KeyValueDbListenerService> _logger;
        public readonly SinkOptions _sinkOptions;
        public KeyValueDbListenerService(ILogger<KeyValueDbListenerService> logger, SinkOptions sinkOptions)
        {
            _logger = logger;
            _sinkOptions = sinkOptions;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(_sinkOptions.DatabaseHost);
            ISubscriber subscriber = redis.GetSubscriber();

            // Nome do stream do Kinesis
            string kinesisStreamName = _sinkOptions.StreamName;

            AmazonKinesisConfig config = new()
            {
                ServiceURL = _sinkOptions.StreamHost,
            };

            BasicAWSCredentials credentials = new(_sinkOptions.ProviderSecretId, _sinkOptions.ProviderSecretKey);

            // Cliente Kinesis
            AmazonKinesisClient kinesisClient = new(config);

            // Inscreve-se para escutar mudanças no Redis
            subscriber.Subscribe(_sinkOptions.DatabaseListeningEvent, async (channel, message) =>
            {
                string eventType = channel.ToString().Split(":").Last();
                if (eventType.Equals("del")) return;

                string key = message.ToString();
                RedisValue value = redis.GetDatabase().StringGet(key);

                // Constrói o evento para o Kinesis
                string json = JsonSerializer.Serialize(new Payload(key, value));
                byte[] payload = Encoding.UTF8.GetBytes(json);

                PutRecordRequest request = new()
                {
                    StreamName = kinesisStreamName,
                    Data = new System.IO.MemoryStream(payload),
                    PartitionKey = _sinkOptions.StreamName
                };

                // Envia o evento para o Kinesis
                await kinesisClient.PutRecordAsync(request);

                _logger.LogInformation($"Evento enviado para Kinesis -> key: {key}, value: {value}, eventType:{eventType}");
            });

            _logger.LogInformation("Monitorando mudanças no Redis...");

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("🛑 RedisListenerService parado!");
            return Task.CompletedTask;
        }
    }
}