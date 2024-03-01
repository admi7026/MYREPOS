using Common;

namespace Catalog.API.Data.Entities
{
    public class Product : BaseEntity
    {
        public string? ProductName { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int UnitInStock { get; set; }
        public virtual Category? Category { get; set; }
    }
}
