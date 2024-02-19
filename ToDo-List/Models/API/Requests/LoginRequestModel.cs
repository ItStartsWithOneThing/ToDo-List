namespace ToDo_List.Models.API.Requests
{
    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FingerPrint { get; set; }
        public string? UserAgent { get; set; }
    }
}
