using Common;

namespace Ordering.API.Data.Entities
{
    public class Order : BaseEntity
    {
        public DateTimeOffset OrderDate { get; set; }
        public string? Note { get; set; }
        public int UserId { get; set; }
        public int StateId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public virtual State? State { get; set; }
    }
}
