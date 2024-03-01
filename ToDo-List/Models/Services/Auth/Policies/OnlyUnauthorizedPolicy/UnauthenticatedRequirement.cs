
using Microsoft.AspNetCore.Authorization;

namespace ToDo_List.Models.Services.Auth.Policies.OnlyUnauthorizedPolicy
{
    public class UnauthenticatedRequirement : IAuthorizationRequirement
    {
    }
}
