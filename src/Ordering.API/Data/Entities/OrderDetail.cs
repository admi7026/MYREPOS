using Common;

namespace Ordering.API.Data.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public virtual Order? Order { get; set; }
    }
}
