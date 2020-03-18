using System.Text.Json.Serialization;

namespace PocketApi
{
    class RequestTokenRequestBody
    {
        [JsonPropertyName("consumer_key")]
        public string ConsumerKey { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
