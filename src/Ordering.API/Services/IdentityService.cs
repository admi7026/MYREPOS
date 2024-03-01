namespace Ordering.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public IdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;

            return Convert.ToInt32(user.Identity.Name);
        }
    }
}
