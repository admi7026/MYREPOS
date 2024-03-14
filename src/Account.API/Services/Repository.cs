using Account.API.Data;
using Account.API.Data.Entities;
using Common;

namespace Account.API.Services
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public IQueryable<Role> Roles => _context.Roles;

        public void Add<T>(T entity) where T : BaseEntity
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public async Task<bool> SaveEntitiesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            _context.Update(entity);
        }
    }
}
