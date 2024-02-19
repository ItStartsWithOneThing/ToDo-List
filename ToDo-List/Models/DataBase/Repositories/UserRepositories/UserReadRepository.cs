
using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.UserRepositories
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(ToDoDbContext context) : base(context) {}

        public async Task<User> GetUserWithSessions(string email, string password)
        {
            var result = await _dbSet.AsNoTracking().Where(x => x.Email == email && x.Password == password).Include(x => x.RefreshSessions).FirstOrDefaultAsync();
            return result;
        }
    }
}
