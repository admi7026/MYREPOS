using Ordering.API.Data.Entities;

namespace Ordering.API.Models
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string? Note { get; set; }
        public int UserId { get; set; }
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public int Total { get; set; }
    }
}
