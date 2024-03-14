using Account.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using Account.API.Models;
using Microsoft.Win32;

namespace Account.IntegrationTests
{
    public class RegisterEndpointTests : IClassFixture<TestWebApplicationFactory<Program>>
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public RegisterEndpointTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = factory.CreateClient();
        }

        [Theory]
        [InlineData("admin","123456", HttpStatusCode.NoContent)]
        [InlineData("admin","", HttpStatusCode.BadRequest)]
        public async Task PostTodoWithValidationProblems(string username, string password, HttpStatusCode statusCode)
        {
            var response = await _httpClient.PostAsJsonAsync("/register", new
            {
                Username = username,
                Password = password
            });

            Assert.Equal(statusCode, response.StatusCode);
        }
    }
}