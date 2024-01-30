
using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Repositories
{
    public class ReadRepository : IReadRepository
    {
        private readonly ToDoDbContext _dbContext;
        private readonly DbSet<TaskCard> _taskCards;

        public ReadRepository(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
            _taskCards = _dbContext.Set<TaskCard>();
        }

        public async Task<IEnumerable<TaskCard>> GetAll()
        {
            return await _taskCards.AsNoTracking().ToListAsync();
        }
    }
}
