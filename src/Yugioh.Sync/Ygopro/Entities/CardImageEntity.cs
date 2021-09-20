using Newtonsoft.Json;

namespace Yugioh.Sync.Ygopro.Entities
{
    public class CardImageEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("image_url_small")]
        public string ImageUrlSmall { get; set; }
    }
}
