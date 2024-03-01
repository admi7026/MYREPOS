namespace Ordering.API.Models
{
    public class CreateOrderRequest
    {
        public string? Note { get; set; }
        public List<OrderDetailDto>? Products { get; set; }
    }
}
