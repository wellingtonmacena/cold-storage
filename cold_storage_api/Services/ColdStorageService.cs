
using cold_storage_api.Models.Dtos;
using cold_storage_api.Models.Entities;
using cold_storage_api.Services.Interfaces;

namespace cold_storage_api.Services
{
    public class ColdStorageService(ILogger<ColdStorageService> logger, CSVService cSVService, IDataCatalogService dataCatalogService, IFileStorageService fileStorageService)
    {
        public void createRecords(string awsAccount, ColdStorage coldStorage, List<Payload> payloads)
        {
            string chunkName =  Guid.NewGuid().ToString().Substring(12);
            string currentDate = DateTime.UtcNow.ToString("yyyy/MM/dd");
            string chunkPath = $"{coldStorage.ApplicationName}/{coldStorage.ServiceName}/{currentDate}/{chunkName}.csv";

            List<Record> records = new();

            payloads.ForEach(item =>
            {
                records.Add(new Record()
                {
                    ChunkPath = chunkPath,
                    CreatedAt = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                });
            });

            byte[] byteFile = cSVService.CreateCsvFile(payloads);

            fileStorageService.CreateFile(awsAccount, coldStorage.FileStorage, byteFile, chunkPath);
            dataCatalogService.PutBatch(awsAccount, coldStorage.DataCatalog, records);
        }
    }
}
