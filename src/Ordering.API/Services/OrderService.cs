using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using EventBus;
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
        private readonly IEventBusService _eventBusService;
        public OrderService(ApplicationDbContext context,
                            IMapper mapper,
                            IIdentityService identityService,
                            IEventBusService eventBusService)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
            _eventBusService = eventBusService;
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

            var orderEvent = new ProcessOrderIntegrationEvent()
            {
                Products = request.Products.Select(x => new ProductInOrderDto()
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                }).ToList()
            };

            _eventBusService.Publish(orderEvent);

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
