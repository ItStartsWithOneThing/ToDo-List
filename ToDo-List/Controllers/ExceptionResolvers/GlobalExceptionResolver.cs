
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ToDo_List.Models.API.Responses;

namespace ToDo_List.Controllers.ExceptionResolvers
{
    public class GlobalExceptionResolver : IExceptionResolver
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionResolver(
            ILogger<GlobalExceptionResolver> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _env = webHostEnvironment;
        }
        public void OnException(ExceptionContext context)
        {
            var id = Guid.NewGuid();
            var errorResponse = new ErrorResponse
            {
                Code = 500,
                ErrorMessage = _env.IsProduction()
                    ? "Internal server error"
                    : context.Exception.ToString()
            };

            _logger.LogCritical(context.Exception, $"ErrorId : {id}");
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
