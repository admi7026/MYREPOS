using Account.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using Account.API.Models;
using Microsoft.Win32;

namespace Account.IntegrationTests
{
    public class LoginEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public LoginEndpointTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task PostValidUser_ReturnOk()
        {
            var response = await _httpClient.PostAsJsonAsync("/login", new
            {
                Username = "test@yourserver.com",
                Password = "123456"
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostInValidUser_ReturnBadRequest()
        {
            var response = await _httpClient.PostAsJsonAsync("/login", new
            {
                Username = "abcdefgh",
                Password = "123456"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}