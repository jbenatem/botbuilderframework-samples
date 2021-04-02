using Newtonsoft.Json;

namespace Jarvis.Domain.Model
{
    public class Button
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
