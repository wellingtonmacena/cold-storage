using cold_storage_api.Models.Enums;

namespace cold_storage_api.Models.Dtos
{
    public class DataStream
    {
        public string StreamName { get; set; }
        public CloudProvider CloudProvider { get; set; }
        public string ServiceRegion { get; set; }
        public string ServiceEndpoint { get; set; }
        public TimeSpan PollingInterval { get; set; }
        public int MaxItemsToFetch { get; set; }
    }
}
