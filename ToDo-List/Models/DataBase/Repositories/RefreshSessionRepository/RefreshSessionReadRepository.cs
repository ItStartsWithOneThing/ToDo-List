
using Microsoft.EntityFrameworkCore;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.RefreshSessionRepository
{
    public class RefreshSessionReadRepository : ReadRepository<RefreshSession>, IRefreshSessionReadRepository
    {
        public RefreshSessionReadRepository(ToDoDbContext context) : base(context) { }

        public async Task<RefreshSession> GetSessionByRefreshToken(Guid refreshToken)
        {
            return await _dbSet.Where(x => x.RefreshToken == refreshToken).FirstOrDefaultAsync();
        }
    }
}
