using System.Security.Claims;

namespace Tigal.Server.Services
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;

        }

        public string GetMobileNumber()
        {
            var mobileid = httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return mobileid;
        }
    }
}
