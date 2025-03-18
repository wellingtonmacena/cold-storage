namespace cold_storage_api.Models.Dtos
{
    public class Payload
    {
        public Payload(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
