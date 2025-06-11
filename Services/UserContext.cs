using System.Security.Claims;

namespace TaskTrackerAPI.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if(context == null || context.User == null)
                       throw new UnauthorizedAccessException("HTTP context or user is not available.");

                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException("User ID is missing from token");

                return userId;
            }
        }
    }
}
