
using AutoMapper;
using ToDo_List.Controllers.Requests;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories;
using ToDo_List.Models.DTO;

namespace ToDo_List.Models.Services
{
    public class TaskCardService : ITaskCardService
    {
        private readonly ILogger<TaskCardService> _logger;
        private readonly IReadRepository _readRepo;
        private readonly IWriteRepository _writeRepo;
        private readonly IMapper _mapper;

        public TaskCardService(
            ILogger<TaskCardService> logger,
            IReadRepository readRepo,
            IWriteRepository writeRepo,
            IMapper mapper)
        {
            _logger = logger;
            _readRepo = readRepo;
            _writeRepo = writeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskCardDto>> GetAllTaskCards()
        {
            try
            {
                var result = await _readRepo.GetAll();
                return _mapper.Map<IEnumerable<TaskCardDto>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
            
        }

        public async Task<TaskCardDto> AddTaskCard(AddNewCardRequestModel taskCardRequest)
        {
            var newCard = _mapper.Map<TaskCard>(taskCardRequest);
            newCard.Id = Guid.NewGuid();
            newCard.EditedDate = DateTime.Now;

            try
            {
                var isCardAdded = await _writeRepo.Add(newCard);

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

        public async Task<bool> UpdateTaskCards(IEnumerable<TaskCardDto> cards)
        {
            try
            {
                var targetCars = _mapper.Map<IEnumerable<TaskCard>>(cards);
                var result = await _writeRepo.Update(targetCars);
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

        public async Task<bool> DeleteTaskCard(Guid id)
        {
            try
            {
                return await _writeRepo.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex, id);
                return false;
            }
        }
    }
}
