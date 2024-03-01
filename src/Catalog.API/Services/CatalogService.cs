using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.API.Data;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CatalogService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var items = await _context.Categories
                .Include(x => x.Products)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();

            return items;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var items = await _context.Products
                .Include(x => x.Category)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();

            return items;
        }
    }
}
