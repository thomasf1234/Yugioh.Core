
using Newtonsoft.Json;
using System;

namespace Yugioh.Sync.Ygopro.Entities
{
    public class CardSetEntity
    {
        [JsonProperty("set_name")]
        public string SetName { get; set; }
        [JsonProperty("set_code")]
        public string SetCode { get; set; }
        [JsonProperty("num_of_cards")]
        public int NumberOfCards { get; set; }
        [JsonProperty("tcg_date")]
        public DateTime TcgDate { get; set; }
    }
}
