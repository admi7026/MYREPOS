
using Account.API.Data;
using Account.API.Data.Entities;
using Account.API.Models;
using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace Account.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository _repository;
        private readonly AudienceSettings _audienceSettings;
        public AccountService(IRepository repository, IOptions<AudienceSettings> options)
        {
            _repository = repository;
            _audienceSettings = options.Value;
        }

        public async Task<TokenResponse?> Login(string username, string password)
        {
            var user = await _repository.Users.Include(x => x.Role)
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(x => x.UserName == username);

            if(user is null)
            {
                return null;
            }

            if (!Md5Helper.VerifyMd5Hash(password, user.Password!))
            {
                return null;
            }

            var timeOutInSeconds = 1 * 60 * 60; // 1 hour
            
            var token = BuildToken(user, timeOutInSeconds);

            return new TokenResponse()
            {
                AccessToken = token,
                ExpiresIn = timeOutInSeconds,
                TokenType = "Bearer"
            };
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            var existUser = await _repository.Users.AnyAsync(x => x.UserName == username);

            if (existUser)
            {
                return false;
            }

            var user = new User()
            {
                UserName = username,
                Password = Md5Helper.GetMd5Hash(password),
                RoleId = 2
            };

            _repository.Add(user);
            
            return await _repository.SaveEntitiesAsync();
        }

        private string BuildToken(User user, int timeOut)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                new Claim(ClaimTypes.Role, user.Role!.RoleName!)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_audienceSettings.Secret!));

            var jwt = new JwtSecurityToken(issuer: _audienceSettings.Iss,
                                           audience: _audienceSettings.Aud,
                                           claims: claims,
                                           notBefore: now,
                                           expires: now.Add(TimeSpan.FromSeconds(timeOut)),
                                           signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
