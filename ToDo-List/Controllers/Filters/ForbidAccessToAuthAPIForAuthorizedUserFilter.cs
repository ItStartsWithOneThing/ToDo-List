
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDo_List.Controllers.Filters
{
    public class ForbidAccessToAuthAPIForAuthorizedUserFilter : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = context.Result = new ContentResult
                {
                    Content = "Forbidden",
                    StatusCode = 403,
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
