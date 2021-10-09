using System.Drawing;

namespace Yugioh.Data.Entities
{
    public class ArtworkEntity
    {
        public int CardId { get; set; }
        public int Ordinal { get; set; }
        public Image Image { get; set; }
    }
}
