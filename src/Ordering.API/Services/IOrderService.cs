using Ordering.API.Models;

namespace Ordering.API.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderDto?> GetOrderAsync(int orderId);
        Task<List<OrderItemDto>> GetOrdersAsync();
    }
}
