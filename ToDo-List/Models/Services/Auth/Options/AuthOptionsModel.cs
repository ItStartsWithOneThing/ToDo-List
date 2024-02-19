
namespace ToDo_List.Models.Services.Auth.Options
{
    public class AuthOptionsModel
    {
        public string Pepper { get; set; }
        public TimeSpan AccessTokenDuration { get; set; }
        public TimeSpan RefreshTokenDuration { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int MaxSessionsAmount { get; set; }
    }
}
