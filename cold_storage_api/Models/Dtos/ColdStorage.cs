namespace cold_storage_api.Models.Dtos
{
    public class ColdStorage
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public DataStream DataStream { get; set; }
        public FileStorage FileStorage { get; set; }
        public DataCatalog DataCatalog { get; set; }
    }
}
