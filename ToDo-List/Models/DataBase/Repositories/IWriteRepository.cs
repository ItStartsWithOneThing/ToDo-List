
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Repositories
{
    public interface IWriteRepository
    {
        public Task<int> Update(IEnumerable<TaskCard> taskCards);
        public Task<bool> Add(TaskCard taskCard);
        public Task<bool> Delete(Guid id);
    }
}
