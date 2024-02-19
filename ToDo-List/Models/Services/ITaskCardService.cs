
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.DTO;

namespace ToDo_List.Models.Services
{
    public interface ITaskCardService
    {
        public Task<IEnumerable<TaskCardDto>> GetAllTaskCards();
        public Task<TaskCardDto> AddTaskCard(AddNewCardRequestModel taskCard);
        public Task<bool> UpdateTaskCards(IEnumerable<TaskCardDto> cards);
        public Task<bool> DeleteTaskCard(Guid id);
    }
}
