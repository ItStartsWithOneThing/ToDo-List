
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDo_List.Controllers.ExceptionResolvers
{
    public interface IExceptionResolver
    {
        public void OnException(ExceptionContext context);
    }
}