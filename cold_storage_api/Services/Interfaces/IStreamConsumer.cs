using cold_storage_api.Models.Dtos;

namespace cold_storage_api.Services.Interfaces
{
    public interface IStreamConsumer
    {
        public void ConsumeStream(ColdStorage coldStorage);
    }
}
