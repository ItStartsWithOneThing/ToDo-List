
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Repositories
{
    public interface IWriteRepository
    {
        public Task Update(TaskCard taskCard);
        public Task Add(TaskCard taskCard);
    }
}
