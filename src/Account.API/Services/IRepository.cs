using Account.API.Data.Entities;
using Common;

namespace Account.API.Services
{
    public interface IRepository
    {
        IQueryable<User> Users { get; }
        IQueryable<Role> Roles { get; }

        void Add<T>(T entity) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        void Delete<T>(T entity) where T : BaseEntity;
        Task<bool> SaveEntitiesAsync();
    }
}
