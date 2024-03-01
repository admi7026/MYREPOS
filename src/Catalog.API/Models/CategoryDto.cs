using Catalog.API.Data.Entities;

namespace Catalog.API.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public int TotalProducts { get; set; }
    }
}
