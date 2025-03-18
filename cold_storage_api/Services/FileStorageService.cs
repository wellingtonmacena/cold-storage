using Amazon;
using Amazon.Kinesis.Model;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using cold_storage_api.Models.Dtos;
using cold_storage_api.Services.Interfaces;

namespace cold_storage_api.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task CreateFile(string awsAccount, FileStorage fileStorage,  byte[] byteFile, string filePath)
        {
            var awsCredentials = new BasicAWSCredentials("test", "test");
            AmazonS3Config amazonS3Config = new()
            {
                ServiceURL = fileStorage.ServiceEndpoint,
                ForcePathStyle = true
            };

            IAmazonS3  _s3Client = new AmazonS3Client(awsCredentials, amazonS3Config);

            try
            {
                var stream = new MemoryStream(byteFile);
                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(stream, fileStorage.BucketName, filePath);
                Console.WriteLine($"Conteúdo enviado para o bucket {fileStorage.BucketName} com o nome {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar o conteúdo: {ex.Message}");
            }

        }

        public Record GetById(string awsAccount, string id)
        {
            throw new NotImplementedException();
        }
    }
}
