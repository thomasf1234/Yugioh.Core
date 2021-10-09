namespace Yugioh.Data.Entities
{
    public class ProductCardEntity
    {
        public string Code { get; set; }
        public string ProductId { get; set; }
        public int CardId { get; set; }
        public int ArtworkOrdinal { get; set; }
        public string Rarity { get; set; }
        public string Passcode { get; set; }
    }
}
