
using ToDo_List.Models.Constants;

namespace ToDo_List.Controllers.Extensions
{
    public static class HttpContextExtension
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            var clientIdClaim = httpContext.User.Claims
                .FirstOrDefault(n => n.Type == ClaimConstants.USER_ID);

            return Guid.Parse(clientIdClaim.Value);
        }
    }
}
