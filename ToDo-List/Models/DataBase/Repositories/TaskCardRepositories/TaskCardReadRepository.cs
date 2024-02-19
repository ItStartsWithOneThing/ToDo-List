
using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.TaskCardRepositories
{
    public class TaskCardReadRepository : ReadRepository<TaskCard>, ITaskCardReadRepository
    {
        public TaskCardReadRepository(ToDoDbContext context) : base(context) {}

        public async Task<TaskCard> GetByIdAndUserId(Guid id, Guid userId)
        {
            return await _dbSet.Where(x => x.Id == id && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskCard>> GetAllUserCards(Guid userId)
        {
            return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
