
namespace ToDo_List.Models.DataBase.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<TaskCard> TaskCards { get; set; }
        public ICollection<RefreshSession> RefreshSessions { get; set; }
    }
}
