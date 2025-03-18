using cold_storage_api.Models.Enums;

namespace cold_storage_api.Models.Dtos
{
    public class DataCatalog
    {
        public string TableName { get; set; }
        public CloudProvider CloudProvider { get; set; }
        public string ServiceRegion { get; set; }
        public string ServiceEndpoint { get; set; }
    }
}