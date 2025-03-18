using cold_storage_api.Models.Dtos;
using cold_storage_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace cold_storage_api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class ColdStorageController
    {
        private IStreamConsumer _streamConsumer { get; set; }
        public ColdStorageController(IStreamConsumer streamConsumer)
        {
            _streamConsumer = streamConsumer;
        }

        [HttpGet] 
        public async Task GetById()
        {

        }

        [HttpPost]
        public async Task StartProssessing()
        {
            ColdStorage coldStorage = new ColdStorage()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                CreatedAt = DateTime.UtcNow,
                DataCatalog = new DataCatalog()
                {
                    Name = "cold-storage-data-catalog",
                    ServiceEndpoint = "http://localhost:4566",
                    ServiceRegion = "us-east-1",
                    CloudProvider = Models.Enums.CloudProvider.AWS
                },
                DataStream = new DataStream()
                {
                    Name = "cold-storage-stream",
                    ServiceEndpoint = "http://localhost:4566",
                    ServiceRegion = "us-east-1",
                    CloudProvider = Models.Enums.CloudProvider.AWS
                },
                FileStorage = new FileStorage()
                {
                    Name = "cold-storage-bucket",
                    ServiceEndpoint = "http://localhost:4566",
                    ServiceRegion = "us-east-1",
                    CloudProvider = Models.Enums.CloudProvider.AWS
                }
            };

            _streamConsumer.ConsumeStream(coldStorage);
        }
        
    }
}
