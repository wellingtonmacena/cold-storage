using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using cold_storage_api.Models.Dtos;
using cold_storage_api.Models.Entities;
using cold_storage_api.Services.Interfaces;

namespace cold_storage_api.Services
{
    public class DataCatalogService : IDataCatalogService
    {
        public Record GetById(string awsAccount, string id)
        {
            throw new NotImplementedException();
        }

        public async Task PutBatch(string awsAccount, DataCatalog dataCatalog, List<Record> records)
        {
            AmazonDynamoDBClient _dynamoDbClient = new(
                new BasicAWSCredentials("test", "test"), // Credenciais fictícias
                new AmazonDynamoDBConfig
                {
                    ServiceURL = dataCatalog.ServiceEndpoint, // LocalStack DynamoDB endpoint

                });

            List<WriteRequest> writeRequests = new();

            // Converte os registros para a estrutura esperada pelo BatchWriteItem
            foreach (Record record in records)
            {
                Dictionary<string, AttributeValue> item = new()
                {
                    { "Id", new AttributeValue { S = record.Id.ToString() } },
                    { "CreatedAt", new AttributeValue { S = record.CreatedAt.ToString("o") } },
                    { "ChunkPath", new AttributeValue { S = record.ChunkPath } }
                };

                writeRequests.Add(new WriteRequest
                {
                    PutRequest = new PutRequest
                    {
                        Item = item
                    }
                });
            }

            BatchWriteItemRequest batchWriteItemRequest = new()
            {
                RequestItems = new Dictionary<string, List<WriteRequest>>
                {
                    { dataCatalog.TableName, writeRequests } // Nome da tabela
                }
            };

            try
            {
                BatchWriteItemResponse batchWriteItemResponse = await _dynamoDbClient.BatchWriteItemAsync(batchWriteItemRequest);

                // Verifica se houve falha na escrita de algum item
                if (batchWriteItemResponse.UnprocessedItems.Count > 0)
                {
                    Console.WriteLine("Some items were not processed. Retry them.");
                }
                else
                {
                    Console.WriteLine("Batch insert successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting batch: {ex.Message}");
            }
        }
    }
}
