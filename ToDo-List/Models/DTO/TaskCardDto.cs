
using ToDo_List.Models.Enums;

namespace ToDo_List.Models.DTO
{
    public class TaskCardDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime EditedDate { get; set; }
        public bool Completed { get; set; } = false;
        public string BackgroundColor { get; set; } = "white";
        public Priority Priority { get; set; } = Priority.Low;
        public bool HasUnsavedChanges { get; set; } = false;
    }
}
