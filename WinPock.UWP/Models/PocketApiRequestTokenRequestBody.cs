using System.Text.Json.Serialization;

namespace WinPock.Models
{
    class PocketApiRequestTokenRequestBody
    {
        [JsonPropertyName("consumer_key")]
        public string ConsumerKey { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
