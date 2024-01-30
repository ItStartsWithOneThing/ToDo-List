namespace ToDo_List.Models.Entities
{
    public class TaskCard
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Completed { get; set; }
        public string BackgroundColor { get; set; } = "white";
        public string TextColor { get; set; } = "black";
        public string TitleColor { get; set; } = "black";
    }
}
