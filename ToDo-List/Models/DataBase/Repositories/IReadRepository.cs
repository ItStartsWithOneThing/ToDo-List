
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Repositories
{
    public interface IReadRepository
    {
        public Task<IEnumerable<TaskCard>> GetAll();
        public Task<TaskCard> GetById(Guid id);
    }
}
