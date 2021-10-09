using System;

namespace Yugioh.Sync.Konami.Entities
{
    public class ProductEntity
    {
        public string Url { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public int SetSize { get; set; }
        public DateTime LaunchDate { get; set; }
    }

}
