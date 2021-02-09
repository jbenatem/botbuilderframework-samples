using Newtonsoft.Json;
using System.Collections.Generic;

namespace CCBot.Domain.Model
{
    public class Card
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public string ImageUrl { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("buttons")]
        public List<Button> buttons { get; set; }
    }
}
