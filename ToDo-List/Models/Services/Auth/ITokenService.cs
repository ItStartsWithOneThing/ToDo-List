
namespace ToDo_List.Models.Services.Auth
{
    public interface ITokenService
    {
        public string? GenerateAccessToken(Guid userId);
    }
}
