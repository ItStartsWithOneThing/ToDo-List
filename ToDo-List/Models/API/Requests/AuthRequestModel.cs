
using System.ComponentModel.DataAnnotations;

namespace ToDo_List.Models.API.Requests
{
    public class AuthRequestModel
    {
        [Required(ErrorMessage = "Field 'Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field 'Password' is required")]
        public string Password { get; set; }
        public string FingerPrint { get; set; }
        public string? UserAgent { get; set; }
    }
}
