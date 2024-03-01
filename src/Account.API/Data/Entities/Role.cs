using Common;

namespace Account.API.Data.Entities
{
    public class Role : BaseEntity
    {
        public string? RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
