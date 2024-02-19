
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.RefreshSessionRepository
{
    public class RefreshSessionWriteRepository : WriteRepository<RefreshSession>, IRefreshSessionWriteRepository
    {
        public RefreshSessionWriteRepository(ToDoDbContext context) : base(context) { }

        public void DeleteRange(IEnumerable<RefreshSession> sessions)
        {
            _dbSet.RemoveRange(sessions);
        }
    }
}
