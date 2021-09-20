using System.Drawing;

namespace Yugioh.Data.Entities
{
    public class ArtworkEntity
    {
        public int ArtworkId { get; set; }
        public int CardId { get; set; }
        public bool Alternate { get; set; }
        public Image Image { get; set; }
    }
}
