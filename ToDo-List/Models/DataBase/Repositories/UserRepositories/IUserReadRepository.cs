
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.UserRepositories
{
    public interface IUserReadRepository : IReadRepository<User>
    {
        public Task<User> GetUserWithSessions(string email, string password);
    }
}
