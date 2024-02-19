
namespace ToDo_List.Models.DTO
{
    public class TokenAggregateDto
    {
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; }
        public TimeSpan RefreshTokenDuration { get; set; }
    }
}
