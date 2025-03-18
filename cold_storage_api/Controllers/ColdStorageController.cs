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

                Stream = new Models.Dtos.Stream()
                {
                    Name = "cold-storage-stream",
                    ProviderEndpoint = "http://localhost:4566",
                    ProviderRegion = "us-east-1",
                }
            };

            _streamConsumer.ConsumeStream(coldStorage);
        }
        
    }
}
