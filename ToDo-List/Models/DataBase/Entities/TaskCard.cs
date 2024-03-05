
using ToDo_List.Models.Enums;

namespace ToDo_List.Models.DataBase.Entities
{
    public class TaskCard
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime EditedDate { get; set; }
        public bool Completed { get; set; } = false;
        public string BackgroundColor { get; set; }
        public Priority Priority { get; set; }
        public User User { get; set; }
    }
}
