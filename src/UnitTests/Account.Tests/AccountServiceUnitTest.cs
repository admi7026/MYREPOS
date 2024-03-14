using Account.API.Models;
using Account.API.Services;
using Account.Tests.Helpers;
using Common;
using Microsoft.Extensions.Options;

namespace Account.Tests
{
    public class AccountServiceUnitTest
    {
        private readonly IAccountService _accountService;
        private static TokenResponse _tokenResponse = new TokenResponse();
        public AccountServiceUnitTest()
        {
            var dbContext = ApplicationDbContextFactory.Create();

            var mockRepository = new Mock<IRepository>();

            mockRepository.Setup(x => x.Users)
                .Returns(dbContext.Users);

            var mockSetting = new Mock<IOptions<AudienceSettings>>();

            mockSetting.Setup(x => x.Value)
                .Returns(new AudienceSettings()
                {
                    Aud = "sample",
                    ClientId = "19611233eb4348fba611ead974aaec8a",
                    Iss = "sample",
                    Secret = "Oh8b3DG7uk5NrqfE_7lRHhHLIOIzZoG1SjzwW3b6gAk"
                });

            _accountService = new AccountService(mockRepository.Object, mockSetting.Object);
        }

        [Theory]
        [InlineData("admin", ApplicationDbContextFactory.ValidPassword, "TOKEN")]
        [InlineData("admin", "9191919", null)]
        public async Task ProcessLogin_ReturnValidData(string username, string password, string accessToken)
        {
            var result = await _accountService.Login(username, password);
            
            Assert.Equal(string.IsNullOrEmpty(accessToken), string.IsNullOrEmpty(result?.AccessToken));
        }
    }
}