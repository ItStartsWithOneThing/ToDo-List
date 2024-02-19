
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.API.Responses;
using ToDo_List.Models.Services.Auth;

namespace ToDo_List.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Login to the system. This method will send back the access-token.
        /// Refresh-token will be attached to http-only cookies automatically.
        /// </summary>
        /// <response code="200">access-token</response>
        /// <response code="400">Request validation error</response>
        /// <response code="401"></response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn([FromBody] LoginRequestModel request)
        {
            request.UserAgent = HttpContext.Request.Headers["User-Agent"];
            var result = await _authService.LogIn(request);

            if (result == null)
            {
                _logger.LogTrace($"Authorization failure with email: {request.Email}");
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Redirect("/Home/Login");
            }

            Response.Cookies.Append("RefreshToken", result.RefreshToken.ToString(), new CookieOptions
            {
                HttpOnly = true,
                MaxAge = result.RefreshTokenDuration,
                SameSite = SameSiteMode.Strict                
            });

            _logger.LogTrace($"User with email: {request.Email} has been authorized");
            return Ok(result.AccessToken);
        }

        /// <summary>
        /// Logout from the system. This method will delete refresh-token from cookies.
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400"></response>
        [AllowAnonymous]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogOut()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            var result = await _authService.LogOut(refreshToken);

            if (result == false)
            {
                return BadRequest();
            }

            Response.Cookies.Delete("RefreshToken");
            return Ok();
        }

        /// <summary>
        /// Refresh access and refresh-tokens by providing
        /// fingerprint and current refresh-token.
        /// Refresh-token will be attached to http-only cookies automatically.
        /// </summary>
        /// <response code="200">access-token</response>
        /// <response code="400">Request validation error</response>
        /// <response code="401"></response>
        [AllowAnonymous]
        [HttpPost("refresh-tokens")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] //TODO
        public async Task<IActionResult> RefreshTokens([FromBody] string fingerPrint)
        {
            var userAgent = HttpContext.Request.Headers["User-Agent"];
            var refreshToken = Request.Cookies["RefreshToken"];

            var result = await _authService.RefreshTokens(fingerPrint, userAgent, refreshToken);
            if(result == null)
            {
                _logger.LogTrace($"Authorization failure with refresh-token: {refreshToken}");
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Redirect("/Home/Login");
            }

            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Append("RefreshToken", result.RefreshToken.ToString(), new CookieOptions
            {
                HttpOnly = true,
                MaxAge = result.RefreshTokenDuration,
                SameSite = SameSiteMode.Strict
            });

            _logger.LogTrace($"User with refresh-token: {result.RefreshToken} has been authorized");
            return Ok(result.AccessToken);
        }
    }
}
