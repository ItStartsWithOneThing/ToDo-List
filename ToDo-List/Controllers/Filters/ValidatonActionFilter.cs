using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ToDo_List.Controllers.Responses;

namespace ToDo_List.Controllers.Filters
{
    public class ValidatonActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                context.Result = new ObjectResult(
                    new ErrorResponse()
                    {
                        Code = 400,
                        Body = context.ModelState.Select(x => new ValidationErrorResponse()
                        {
                            Field = x.Key,
                            Details = x.Value?.Errors.Select(x => x.ErrorMessage).FirstOrDefault()!
                        }),
                        ErrorMessage = "Validation error"
                    });
            }
        }
    }
}
