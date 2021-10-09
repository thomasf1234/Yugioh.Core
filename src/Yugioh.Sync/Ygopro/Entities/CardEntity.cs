using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yugioh.Sync.Ygopro.Entities
{
    public class CardEntity
    {
        // passcode
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("desc")]
        public string Desc { get; set; }
        [JsonProperty("atk")]
        public string Atk { get; set; }
        [JsonProperty("def")]
        public string Def { get; set; }
        [JsonProperty("race")]
        public string Race { get; set; }
        [JsonProperty("attribute")]
        public string Attribute { get; set; }
        [JsonProperty("level")]
        public int? Level { get; set; }
        [JsonProperty("rank")]
        public int? Rank { get; set; }
        [JsonProperty("scale")]
        public int? Scale { get; set; }
        [JsonProperty("linkval")]
        public int? LinkVal { get; set; }
        [JsonProperty("linkmarkers")]
        public List<string> LinkMarkers { get; set; }
        [JsonProperty("archetype")]
        public string Archetype { get; set; }
        [JsonProperty("card_images")]
        public List<CardImageEntity> CardImageEntities { get; set; }
        [JsonProperty("card_sets")]
        public List<CardSetInfoEntity> CardSetInfoEntities { get; set; }
    }
}
