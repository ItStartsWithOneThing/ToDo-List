
namespace ToDo_List.Models.DataBase.Entities
{
    public class RefreshSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RefreshToken { get; set; }
        public string FingerPrint { get; set; }
        public string UserAgent { get; set; }
        public DateTime ExpiresIn { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}
