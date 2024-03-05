
using Microsoft.AspNetCore.Mvc.Filters;
using ToDo_List.Controllers.ExceptionResolvers;

namespace ToDo_List.Controllers.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var globalResolver = context.HttpContext.RequestServices.GetService<IExceptionResolver>();
            globalResolver.OnException(context);
        }
    }
}
