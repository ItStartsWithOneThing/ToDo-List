
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.Requests;

namespace ToDo_List.Models.Services
{
    public interface ITaskCardService
    {
        public Task<IEnumerable<TaskCard>> GetAllTaskCards();
        public Task<TaskCard> AddTaskCard(AddNewCardRequestModel taskCard);
        public Task SetTaskCardAsCompleted(Guid id);
        public Task SetTaskCardTitle(Guid id, string newTitle);
        public Task DeleteTaskCard(Guid id);
    }
}
