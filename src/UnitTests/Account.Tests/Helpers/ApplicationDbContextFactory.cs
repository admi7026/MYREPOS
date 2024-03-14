using Account.API.Data;
using Account.API.Data.Entities;
using Account.API.Models;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;

namespace Account.Tests.Helpers
{
    internal class ApplicationDbContextFactory
    {
        private static ApplicationDbContext? _context;
        public const string ValidPassword = "password";
        public static ApplicationDbContext Create()
        {
            if (_context != null)
            {
                return _context;
            }

            return SetupDbContext();
        }

        private static ApplicationDbContext SetupDbContext()
        {
            var mock = GetDbContextMock();
            
            return _context = mock.Object;
        }

        public static Mock<ApplicationDbContext> GetDbContextMock()
        {
            string hashPassword = Md5Helper.GetMd5Hash(ValidPassword);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

            var mock = new Mock<ApplicationDbContext>(options);
            var role = new Role() { Id = 1, RoleName = "Admin" };
            var users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    UserName = "admin",
                    Password = hashPassword,
                    RoleId = 1,
                    Role = role
                }
            };

            var roles = new List<Role>()
            {
                role
            };

            mock.Setup(x => x.Users)
                .ReturnsDbSet(users);

            mock.Setup(x => x.Roles)
               .ReturnsDbSet(roles);

            return mock;
        }
    }
}
