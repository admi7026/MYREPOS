using Account.API.Models;

namespace Account.API.Services
{
    public interface IAccountService
    {
        Task<TokenResponse?> Login(string username, string password);
        Task<bool> RegisterUser(string username, string password);
    }
}
