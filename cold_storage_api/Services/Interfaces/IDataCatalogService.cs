using cold_storage_api.Models.Dtos;
using cold_storage_api.Models.Entities;

namespace cold_storage_api.Services.Interfaces
{
    public interface IDataCatalogService
    {
        public Record GetById(string awsAccount, string id);
        public  Task PutBatch(string awsAccount, DataCatalog dataCatalog, List<Record> records);
    }
}
