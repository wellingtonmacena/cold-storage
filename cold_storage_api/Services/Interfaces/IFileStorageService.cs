using Amazon.Kinesis.Model;
using cold_storage_api.Models.Dtos;

namespace cold_storage_api.Services.Interfaces
{
    public interface IFileStorageService
    {
        public Record GetById(string awsAccount, string id);
        public Task CreateFile(string awsAccount, FileStorage fileStorage, byte[] byteFile, string filePath);
    }
}
