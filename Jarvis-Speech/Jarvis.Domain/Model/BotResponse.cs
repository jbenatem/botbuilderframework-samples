using Newtonsoft.Json;
using System.Collections.Generic;

namespace Jarvis.Domain.Model
{
    public class BotResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public List<string> Text { get; set; }

        [JsonProperty("quick_replies")]
        public List<string> QuickReplies { get; set; }

        [JsonProperty("cards")]
        public List<Card> Cards { get; set; }
    }
}
