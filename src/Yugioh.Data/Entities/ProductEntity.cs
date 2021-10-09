using System;

namespace Yugioh.Data.Entities
{
    public class ProductEntity
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public int SetSize { get; set; }
        public DateTime LaunchDate { get; set; }
    }
}
