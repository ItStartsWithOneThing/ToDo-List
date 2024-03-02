
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_List.Controllers.Filters;
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
        /// Creates a new user.
        /// Refresh and access tokens will be attached to http-only cookies automatically.
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Request validation error</response>
        /// <response code="401"></response>
        [AllowAnonymous]
        [ForbidAccessToAPIForAuthorizedUserFilter]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        {
            request.UserAgent = HttpContext.Request.Headers["User-Agent"];
            var result = await _authService.Register(request);

            if (result == null)
            {
                _logger.LogInformation($"Authorization failure with email: {request.Email}");
                return BadRequest(request);
            }

            Response.Cookies.Append("RefreshToken", result.RefreshToken.ToString(), new CookieOptions
            {
                MaxAge = result.RefreshTokenDuration
            });

            Response.Cookies.Append("AccessToken", result.AccessToken, new CookieOptions
            {
                MaxAge = result.AccessTokenDuration
            });

            _logger.LogInformation($"User with email: {request.Email} has been authorized");
            return Ok();
        }

        /// <summary>
        /// Login to the system. This method will add access and refresh tokens in cookies.
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Request validation error</response>
        /// <response code="401"></response>
        [AllowAnonymous]
        [ForbidAccessToAPIForAuthorizedUserFilter]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LogIn([FromBody] AuthRequestModel request)
        {
            request.UserAgent = HttpContext.Request.Headers["User-Agent"];
            var result = await _authService.LogIn(request);

            if (result == null)
            {
                _logger.LogInformation($"Authorization failure with email: {request.Email}");
                return BadRequest(request);
            }

            Response.Cookies.Append("RefreshToken", result.RefreshToken.ToString(), new CookieOptions
            {
                MaxAge = result.RefreshTokenDuration             
            });

            Response.Cookies.Append("AccessToken", result.AccessToken, new CookieOptions
            {
                MaxAge = result.AccessTokenDuration
            });

            _logger.LogInformation($"User with email: {request.Email} has been authorized");
            return Ok();
        }

        /// <summary>
        /// Logout from the system. This method will delete refresh-token from cookies.
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400"></response>
        [HttpGet("logout")]
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

            Response.Cookies.Delete("AccessToken");
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
                return Unauthorized();
            }

            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Append("RefreshToken", result.RefreshToken.ToString(), new CookieOptions
            {
                MaxAge = result.RefreshTokenDuration
            });

            Response.Cookies.Append("AccessToken", result.AccessToken, new CookieOptions
            {
                MaxAge = result.AccessTokenDuration
            });

            _logger.LogTrace($"User with refresh-token: {result.RefreshToken} has been authorized");
            return Ok();
        }
    }
}
