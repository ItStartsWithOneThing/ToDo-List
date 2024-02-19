
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.TaskCardRepositories
{
    public interface ITaskCardReadRepository : IReadRepository<TaskCard>
    {
        public Task<TaskCard> GetByIdAndUserId(Guid id, Guid userId);
        public Task<IEnumerable<TaskCard>> GetAllUserCards(Guid userId);
    }
}
