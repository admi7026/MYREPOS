using Catalog.API.Models;

namespace Catalog.API.Services
{
    public interface ICatalogService
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<List<CategoryDto>> GetCategoriesAsync();
    }
}
