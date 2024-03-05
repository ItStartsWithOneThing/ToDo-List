
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.RefreshSessionRepository
{
    public interface IRefreshSessionReadRepository : IReadRepository<RefreshSession>
    {
        public Task<RefreshSession> GetSessionByRefreshToken(Guid refreshToken);
    }
}
