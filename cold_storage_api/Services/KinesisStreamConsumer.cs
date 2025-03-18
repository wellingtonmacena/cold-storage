using Amazon;
using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Amazon.Runtime;
using cold_storage_api.Models.Dtos;
using cold_storage_api.Services.Interfaces;
using System.Text;

namespace cold_storage_api.Services
{
    public class KinesisStreamConsumer : IStreamConsumer
    {
        public async void ConsumeStream(ColdStorage coldStorage)
        {
            AmazonKinesisClient kinesisClient = new(
            new BasicAWSCredentials("test", "test"), // Use LocalStack credentials
            new AmazonKinesisConfig
            {
                ServiceURL = coldStorage.Stream.ProviderEndpoint, // LocalStack endpoint
                //RegionEndpoint = RegionEndpoint.GetBySystemName(coldStorage.Stream.ProviderRegion) // Match your setup
            }
            );


            Console.WriteLine("Fetching shards...");

            var describeStreamResponse = await kinesisClient.DescribeStreamAsync(new DescribeStreamRequest
            {
                StreamName = coldStorage.Stream.Name,
                
            });

            var shardId = describeStreamResponse.StreamDescription. Shards[0].ShardId;

            var getShardIteratorResponse = await kinesisClient.GetShardIteratorAsync(new GetShardIteratorRequest
            {
                StreamName = coldStorage.Stream.Name,
                ShardId = shardId,
                ShardIteratorType = ShardIteratorType.TRIM_HORIZON, 
            });

            string shardIterator = getShardIteratorResponse.ShardIterator;

            Console.WriteLine("Listening for records...");
            while (!string.IsNullOrEmpty(shardIterator))
            {
                var getRecordsResponse = await kinesisClient.GetRecordsAsync(new GetRecordsRequest
                {
                    ShardIterator = shardIterator,
                    Limit = 10,
                  
                });

                foreach (var record in getRecordsResponse.Records)
                {
                    string data = Encoding.UTF8.GetString(record.Data.ToArray());
                    Console.WriteLine($"Received record: {data}");
                }

                shardIterator = getRecordsResponse.NextShardIterator;
                await Task.Delay(2000); // Wait before the next poll
            }
        }
    }
    
}
