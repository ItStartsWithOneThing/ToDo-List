
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.Base;

namespace ToDo_List.Models.DataBase.Repositories.TaskCardRepositories
{
    public class TaskCardWriteRepository : WriteRepository<TaskCard>, ITaskCardWriteRepository
    {
        public TaskCardWriteRepository(ToDoDbContext context) : base(context) {}
    }
}
