using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Data;
using Ordering.API.Data.Entities;
using Ordering.API.Models;

namespace Ordering.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        public OrderService(ApplicationDbContext context, IMapper mapper, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<int> CreateOrderAsync(CreateOrderRequest request)
        {
            var userId = _identityService.GetUserId();

            var order = _mapper.Map<Order>(request);
            order.UserId = userId;

            foreach(var product in request.Products)
            {
                var detail = _mapper.Map<OrderDetail>(product);

                order.OrderDetails.Add(detail);
            }

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId)
        {
            var userId = _identityService.GetUserId();

            var order = await _context.Orders
                .Include(x => x.OrderDetails)
                .Include(x => x.State)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);

            return order;
        }

        public async Task<List<OrderItemDto>> GetOrdersAsync()
        {
            var userId = _identityService.GetUserId();

            var orders = await _context.Orders
                .Include(x => x.OrderDetails)
                .Include(x => x.State)
                .ProjectTo<OrderItemDto>(_mapper.ConfigurationProvider)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return orders;
        }
    }
}
