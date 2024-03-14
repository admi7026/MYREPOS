using Account.API.Data;
using Account.API.Data.Entities;
using Account.API.Models;
using Account.API.Services;
using Account.Tests.Helpers;
using Common;
using Microsoft.Extensions.Options;

namespace Account.Tests
{
    public class RepositoryUnitTest
    {
        private readonly Repository repository;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        public RepositoryUnitTest()
        {
            _mockDbContext = ApplicationDbContextFactory.GetDbContextMock();
         
            repository = new Repository(_mockDbContext.Object);
        }

        [Fact]
        public void GetExistingUser_ReturnValue()
        {
            var result = repository.Users.FirstOrDefault(x => x.Id == 1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("admin", result.UserName);
        }

        [Fact]
        public void GetNotExistingUser_ReturnNull()
        {
            var result = repository.Users.FirstOrDefault(x => x.Id == 0);

            Assert.Null(result);
        }

        [Fact]
        public void CanAddUser_CallOneTime()
        {
            var user = new User()
            {
                Id = 999,
                UserName = "Test",
                RoleId = 1
            };

            repository.Add(user);

            _mockDbContext.Verify(m => m.Add(user), Times.Once);
        }
    }
}