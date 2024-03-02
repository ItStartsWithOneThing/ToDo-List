
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.DTO;

namespace ToDo_List.Models.Services.Auth
{
    public interface IAuthService
    {
        public Task<TokenAggregateDto> Register(RegisterRequestModel model);
        public Task<TokenAggregateDto> LogIn(AuthRequestModel model);
        public Task<bool> LogOut(string refreshToken);
        public Task<TokenAggregateDto> RefreshTokens(string fingerPrint, string userAgent, string refreshToken);
    }
}
