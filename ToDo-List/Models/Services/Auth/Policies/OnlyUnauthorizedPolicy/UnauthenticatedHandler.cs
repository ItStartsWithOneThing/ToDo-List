
using Microsoft.AspNetCore.Authorization;

namespace ToDo_List.Models.Services.Auth.Policies.OnlyUnauthorizedPolicy
{
    public class UnauthenticatedHandler : AuthorizationHandler<UnauthenticatedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UnauthenticatedRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
