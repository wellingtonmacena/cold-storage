namespace cold_storage_api.Models.Dtos
{
    public record RecordDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ChunkPath { get; set; }
    }
}
