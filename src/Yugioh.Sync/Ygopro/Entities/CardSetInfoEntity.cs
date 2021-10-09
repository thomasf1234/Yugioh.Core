
using Newtonsoft.Json;

namespace Yugioh.Sync.Ygopro.Entities
{
    public class CardSetInfoEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("set_name")]
        public string SetName { get; set; }
        [JsonProperty("set_code")]
        public string SetCode { get; set; }
        [JsonProperty("set_rarity")]
        public string SetRarity { get; set; }
        [JsonProperty("set_rarity_code")]
        public string SetRarityCode { get; set; }
        [JsonProperty("set_price")]
        public string SetPrice { get; set; }
    }
}
