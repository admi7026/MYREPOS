using Common;

namespace Catalog.API.Data.Entities
{
    public class Category : BaseEntity
    {
        public string? CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
