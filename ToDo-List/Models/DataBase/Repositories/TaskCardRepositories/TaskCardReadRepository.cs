
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.TaskCardRepositories
{
    public class TaskCardReadRepository : ReadRepository<TaskCard>, ITaskCardReadRepository
    {
        public TaskCardReadRepository(ToDoDbContext context) : base(context) {}
    }
}
