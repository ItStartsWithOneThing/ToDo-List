
using System.ComponentModel.DataAnnotations;

namespace ToDo_List.Models.API.Requests
{
    public class RegisterRequestModel : AuthRequestModel
    {
        [Required(ErrorMessage = "Field 'Name' is required")]
        public string Name { get; set; }

        [Compare("Password", ErrorMessage = "Wrong password")]
        public string PasswordRepeat { get; set; }
    }
}
