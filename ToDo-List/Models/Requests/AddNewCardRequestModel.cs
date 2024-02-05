
using ToDo_List.Models.Enums;

namespace ToDo_List.Models.Requests
{
    public class AddNewCardRequestModel
    {
        public string? Title { get; set; } 
        public string? Text { get; set; }
        public string BackgroundColor { get; set; }
        public Priority Priority { get; set; }
    }
}
