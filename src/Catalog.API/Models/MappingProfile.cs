using AutoMapper;
using Catalog.API.Data.Entities;

namespace Catalog.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.CategoryName, opt =>
                {
                    opt.PreCondition(x => x.Category != null);
                    opt.MapFrom(src => src.Category!.CategoryName);
                });

            CreateMap<Category, CategoryDto>()
                .ForMember(x => x.TotalProducts, opt =>
                {
                    opt.PreCondition(x => x.Products != null);
                    opt.MapFrom(src => src.Products!.Count());
                });
        }
    }
}
