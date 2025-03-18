using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.Runtime;
using cold_storage_api.Models.Dtos;
using cold_storage_api.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace cold_storage_api.Services
{
    public class KinesisStreamConsumer(ILogger<KinesisStreamConsumer> _logger, ColdStorageService _coldStorageService) : IStreamConsumer
    {

        public async void ConsumeStream(ColdStorage coldStorage)
        {
            AmazonKinesisClient kinesisClient = new(
                new BasicAWSCredentials("test", "test"), // Use LocalStack credentials
                new AmazonKinesisConfig
                {
                    ServiceURL = coldStorage.DataStream.ServiceEndpoint, // LocalStack endpoint
                }
            );

            _logger.LogInformation("Fetching shards...");

            try
            {
                DescribeStreamResponse describeStreamResponse = await kinesisClient.DescribeStreamAsync(new DescribeStreamRequest
                {
                    StreamName = coldStorage.DataStream.StreamName,
                });

                string shardId = describeStreamResponse.StreamDescription.Shards[0].ShardId;

                GetShardIteratorResponse getShardIteratorResponse = await kinesisClient.GetShardIteratorAsync(new GetShardIteratorRequest
                {
                    StreamName = coldStorage.DataStream.StreamName,
                    ShardId = shardId,
                    ShardIteratorType = ShardIteratorType.TRIM_HORIZON,
                });

                string shardIterator = getShardIteratorResponse.ShardIterator;

                _logger.LogInformation("Listening for records...");

                while (!string.IsNullOrEmpty(shardIterator))
                {
                    GetRecordsResponse getRecordsResponse = await kinesisClient.GetRecordsAsync(new GetRecordsRequest
                    {
                        ShardIterator = shardIterator,
                        Limit = coldStorage.DataStream.MaxItemsToFetch,
                    });

                    List<Payload> payloads = new List<Payload>();
                    foreach (Record? record in getRecordsResponse.Records)
                    {
                        string data = Encoding.UTF8.GetString(record.Data.ToArray());
                        Console.WriteLine(data);
                        var json = JsonSerializer.Deserialize<Payload>(data);
                        payloads.Add(json);

                        _logger.LogInformation($"Received record: {data}");
                    }

                    if (payloads.Count > 0)
                    {
                        _coldStorageService.createRecords("1", coldStorage, payloads);
                        _logger.LogInformation($"Created {payloads.Count} records in the cold storage");
                    }

                    shardIterator = getRecordsResponse.NextShardIterator;
                    _logger.LogInformation("Waiting before the next poll...");
                    await Task.Delay(coldStorage.DataStream.PollingInterval); // Wait before the next poll
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming the stream");
            }
        }
    }
}
