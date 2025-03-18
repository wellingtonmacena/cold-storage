namespace cold_storage_database_stream_sink.src.Configs.Options
{
    public class SinkOptions
    {
        public string DatabaseHost { get; set; }
        public string DatabaseListeningEvent { get; set; }
        public string StreamHost { get; set; }
        public string ProviderStreamRegion { get; set; }
        public string ProviderSecretId { get; set; }
        public string ProviderSecretKey { get; set; }
        public string StreamName { get; set; }

    }
}
