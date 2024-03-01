
namespace ToDo_List.Controllers.Middlewares
{
    // This Middleware takes access token from http-only cookie and puts it into header
    public class TokenHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);

                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");    

            await _next.Invoke(context);
        }
    }
}
