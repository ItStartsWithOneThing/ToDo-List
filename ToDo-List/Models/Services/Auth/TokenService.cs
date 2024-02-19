
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDo_List.Models.Constants;
using ToDo_List.Models.Services.Auth.Options;

namespace ToDo_List.Models.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly AuthOptionsModel _authOptions;

        public TokenService(
            ILogger<TokenService> logger,
            IOptions<AuthOptionsModel> authOptions)
        {
            _logger = logger;
            _authOptions = authOptions.Value;
        }

        public string? GenerateAccessToken(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _authOptions.Issuer,
                Audience = _authOptions.Audience,
                Subject = GenerateClaimsIdentity(userId),
                Expires = DateTime.Now + _authOptions.AccessTokenDuration,
                SigningCredentials = new SigningCredentials(GetKey(), SecurityAlgorithms.HmacSha256Signature)
            };

            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Cannot create access token for user with id: {userId}");
                return null;
            }
        }

        private ClaimsIdentity GenerateClaimsIdentity(Guid userId)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimConstants.USER_ID, userId.ToString()));

            return new ClaimsIdentity(claims);
        }

        private SymmetricSecurityKey GetKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecretKey));
        }
    }
}
