
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.RefreshSessionRepository
{
    public interface IRefreshSessionWriteRepository : IWriteRepository<RefreshSession>
    {
        public void DeleteRange(IEnumerable<RefreshSession> sessions);
    }
}
