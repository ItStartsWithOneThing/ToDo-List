
using ToDo_List.Models.Enums;

namespace ToDo_List.Models.DataBase.Entities
{
    public class TaskCard
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime Edited { get; set; }
        public bool Completed { get; set; } = false;
        public string BackgroundColor { get; set; } = "white";
        public Priority Priority { get; set; } = Priority.Low;
    }
}
