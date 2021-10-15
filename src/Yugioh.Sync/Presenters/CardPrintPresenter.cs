using System.Collections.Generic;

namespace Yugioh.Sync.Presenters
{
    public class CardPrintPresenter
    {
        public int CardId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Attribute { get; set; }
        public byte? Level { get; set; }
        public byte? Rank { get; set; }
        public byte? PendulumScale { get; set; }
        public byte? LinkRating { get; set; }
        public List<string> LinkArrows { get; set; }
        public string Property { get; set; }
        public List<string> MonsterTypes { get; set; }
        public string Race { get; set; }
        public List<string> Abilities { get; set; }
        public string Attack { get; set; }
        public string Defense { get; set; }
        public string Description { get; set; }
        public string PendulumDescription { get; set; }
        public List<string> Sets { get; set; }
    }
}
