
namespace ToDo_List.Models.DataBase.Repositories.Base
{
    public interface IReadRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAllAsNoTracking();
        public Task<IEnumerable<TEntity>> GetAll();
        public Task<TEntity> GetById(Guid id);
    }
}
