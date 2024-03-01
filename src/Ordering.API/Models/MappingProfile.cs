using AutoMapper;
using Ordering.API.Data.Entities;

namespace Ordering.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(x => x.StateId, opt => opt.MapFrom(src => 1))
                .ForMember(x => x.OrderDate, opt => opt.MapFrom(src => DateTimeOffset.Now.ToUniversalTime()));

            CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();

            CreateMap<Order, OrderDto>()
                .ForMember(x => x.StateName, opt => opt.MapFrom(src => src.State!.StateName))
                .ForMember(x => x.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToLocalTime()))
                .ForMember(x => x.Products, opt => opt.MapFrom(src => src.OrderDetails));

            CreateMap<Order, OrderItemDto>()
                .ForMember(x => x.StateName, opt => opt.MapFrom(src => src.State!.StateName))
                .ForMember(x => x.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToLocalTime()))
                .ForMember(x => x.Total, opt => opt.MapFrom(src => src.OrderDetails.Sum(x => x.Quantity * x.Price)));
        }
    }
}
