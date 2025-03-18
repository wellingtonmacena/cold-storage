namespace cold_storage_api.Models.Entities
{
    public class Record
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ChunkPath { get; set; }
    }
}
