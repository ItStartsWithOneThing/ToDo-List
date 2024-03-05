
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.DTO;

namespace ToDo_List.Models.Services
{
    public interface ITaskCardService
    {
        public Task<IEnumerable<TaskCardDto>> GetAllTaskCards(Guid userId);
        public Task<TaskCardDto> AddTaskCard(AddNewCardRequestModel taskCardRequest, Guid userId);
        public Task<bool> UpdateTaskCards(IEnumerable<TaskCardDto> cards, Guid userId);
        public Task<bool> DeleteTaskCard(Guid id, Guid userId);
    }
}
