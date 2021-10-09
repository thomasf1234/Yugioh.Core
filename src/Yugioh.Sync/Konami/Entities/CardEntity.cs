using System.Collections.Generic;

namespace Yugioh.Sync.Konami.Entities
{
    public enum LinkMarker
    {
        BottomLeft = 1,
        Bottom = 2,
        BottomRight = 3,
        Left = 4,

        Right = 6,
        TopLeft = 7,
        Top = 8,
        TopRight = 9
    }

    public class CardEntity
    {
        public string Url { get; set; }
        public int CardId { get; set; }
        public string Name { get; set; }
        public string Attribute { get; set; }
        public int? Level { get; set; }
        public int? Rank { get; set; }
        public int? PendulumScale { get; set; }
        public int? Link { get; set; }
        public List<LinkMarker> LinkMarkers { get; set; }
        public string Property { get; set; }
        public List<string> MonsterTypes { get; set; }  
        public string Atk { get; set; }
        public string Def { get; set; }
        public string Description { get; set; }
        public string PendulumDescription { get; set; }
    }
}
