using Common;

namespace Account.API.Data.Entities
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int RoleId { get; set; }

        public virtual Role? Role { get; set; }
    }
}
