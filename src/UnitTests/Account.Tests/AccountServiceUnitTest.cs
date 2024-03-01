using Account.API.Data;
using Account.API.Data.Entities;
using Account.API.Models;
using Account.API.Services;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;

namespace Account.Tests
{
    public class AccountServiceUnitTest
    {
        private readonly IAccountService _accountService;
        private readonly string _validPassword;
        public AccountServiceUnitTest()
        {
            _validPassword = "password";
            string hashPassword = Md5Helper.GetMd5Hash(_validPassword);

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

            var mockSetting = new Mock<IOptions<AudienceSettings>>();

            mockSetting.Setup(x => x.Value)
                .Returns(new AudienceSettings()
                {
                    Aud = "sample",
                    ClientId = "19611233eb4348fba611ead974aaec8a",
                    Iss = "sample",
                    Secret = "Oh8b3DG7uk5NrqfE_7lRHhHLIOIzZoG1SjzwW3b6gAk"
                });

            _accountService = new AccountService(mock.Object, mockSetting.Object);
        }

        [Test]
        public async Task CanLoginWithValidDataTest()
        {
            var result = await _accountService.Login("admin", _validPassword);
            
            Assert.NotNull(result);
        }

        [Test]
        public async Task CanNotLoginWithInValidDataTest()
        {
            var result = await _accountService.Login("admin", "9191919");

            Assert.Null(result);
        }
    }
}