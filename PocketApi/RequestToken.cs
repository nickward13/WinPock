using System.Text.Json.Serialization;

namespace PocketApi
{
    public class RequestToken
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("state")]
        public object State { get; set; }
    }
}
