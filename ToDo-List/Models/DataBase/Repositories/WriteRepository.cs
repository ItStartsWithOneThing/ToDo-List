
using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Repositories
{
    public class WriteRepository : IWriteRepository
    {
        private readonly ToDoDbContext _dbContext;
        private readonly DbSet<TaskCard> _taskCards;

        public WriteRepository(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
            _taskCards = _dbContext.Set<TaskCard>();
        }

        public async Task Update(TaskCard taskCard)
        {
            _taskCards.Entry(taskCard).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Add(TaskCard taskCard)
        {
            _taskCards.Add(taskCard);
            return await _dbContext.SaveChangesAsync() != 0;
        }

        public async Task Delete(Guid id)
        {
            var taskCardToDelete = new TaskCard() { Id = id };

            _dbContext.Entry(taskCardToDelete).State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }
    }
}
