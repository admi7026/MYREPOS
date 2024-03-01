namespace Catalog.API.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int UnitInStock { get; set; }
        public string? CategoryName { get; set; }
    }
}
