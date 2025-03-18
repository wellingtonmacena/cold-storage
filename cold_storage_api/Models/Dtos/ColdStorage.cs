namespace cold_storage_api.Models.Dtos
{
    public class ColdStorage
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ApplicationName { get; set; }
        public string ServiceName { get; set; }
        public DataStream DataStream { get; set; }
        public FileStorage FileStorage { get; set; }
        public DataCatalog DataCatalog { get; set; }
    }
}
