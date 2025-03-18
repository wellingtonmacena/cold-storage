using StackExchange.Redis;

namespace cold_storage_database_stream_sink.Models.Contracts
{
    public class Payload
    {
        public Payload(string key, RedisValue value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
