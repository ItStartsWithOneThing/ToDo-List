
namespace ToDo_List.Models.DataBase.Repositories.Base
{
    public interface IWriteRepository<TEntity>
    {
        public Task<int> UpdateRange(IEnumerable<TEntity> entities);
        public void Update(TEntity entity);
        public void Add(TEntity entity);
        public void Delete(TEntity entity);
        public Task<bool> SaveChangesAsync();
    }
}
