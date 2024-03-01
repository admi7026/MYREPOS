namespace Ordering.API.Models
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
