
using Microsoft.EntityFrameworkCore;

namespace ToDo_List.Models.DataBase.Repositories.Base
{
    public abstract class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private protected readonly DbSet<TEntity> _dbSet;

        public ReadRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsNoTracking()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
