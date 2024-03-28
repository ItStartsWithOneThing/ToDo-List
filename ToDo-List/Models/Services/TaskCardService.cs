
using AutoMapper;
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories.TaskCardRepositories;
using ToDo_List.Models.DTO;

namespace ToDo_List.Models.Services
{
    public class TaskCardService : ITaskCardService
    {
        private readonly ILogger<TaskCardService> _logger;
        private readonly ITaskCardReadRepository _taskCardReadRepo;
        private readonly ITaskCardWriteRepository _taskCardWriteRepo;
        private readonly IMapper _mapper;

        public TaskCardService(
            ILogger<TaskCardService> logger,
            ITaskCardReadRepository taskCardReadRepo,
            ITaskCardWriteRepository taskCardWriteRepo,
            IMapper mapper)
        {
            _logger = logger;
            _taskCardReadRepo = taskCardReadRepo;
            _taskCardWriteRepo = taskCardWriteRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskCardDto>> GetAllTaskCards(Guid userId)
        {
            try
            {
                var result = await _taskCardReadRepo.GetAllUserCards(userId);
                return _mapper.Map<IEnumerable<TaskCardDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
            
        }

        public async Task<TaskCardDto> AddTaskCard(AddNewCardRequestModel taskCardRequest, Guid userId)
        {
            var newCard = _mapper.Map<TaskCard>(taskCardRequest);
            newCard.Id = Guid.NewGuid();
            newCard.UserId = userId;

            DateTime currentDate = DateTime.Now;
            DateTime currentDateWithoutMilliseconds = currentDate.AddTicks(-(currentDate.Ticks % TimeSpan.TicksPerSecond)); // Removing milliseconds

            newCard.EditedDate = currentDateWithoutMilliseconds;

            try
            {
                _taskCardWriteRepo.Add(newCard);
                var isCardAdded = await _taskCardWriteRepo.SaveChangesAsync();

                if (isCardAdded)
                {
                    return _mapper.Map<TaskCardDto>(newCard);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<bool> UpdateTaskCards(IEnumerable<TaskCardDto> cards, Guid userId)
        {
            try
            {
                var targetCards = _mapper.Map<IEnumerable<TaskCard>>(cards);
                foreach (var targetCard in targetCards)
                {
                    targetCard.UserId = userId;
                }

                var result = await _taskCardWriteRepo.UpdateRange(targetCards);

                if(result < cards.Count())
                {
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex, cards.Count());
                return false;
            }
        }

        public async Task<bool> DeleteTaskCard(Guid id, Guid userId)
        {
            try
            {
                var taskCardToDelete = await _taskCardReadRepo.GetByIdAndUserId(id, userId);
                if(taskCardToDelete != null)
                {
                    _taskCardWriteRepo.Delete(taskCardToDelete);
                    return await _taskCardWriteRepo.SaveChangesAsync();
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, id);
                return false;
            }
        }
    }
}
