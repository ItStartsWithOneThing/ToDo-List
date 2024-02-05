
using AutoMapper;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories;
using ToDo_List.Models.Requests;

namespace ToDo_List.Models.Services
{
    public class TaskCardService : ITaskCardService
    {
        private readonly IReadRepository _readRepo;
        private readonly IWriteRepository _writeRepo;
        private readonly IMapper _mapper;

        public TaskCardService(
            IReadRepository readRepo,
            IWriteRepository writeRepo,
            IMapper mapper)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskCard>> GetAllTaskCards()
        {
            return await _readRepo.GetAll();
        }

        public async Task<TaskCard> AddTaskCard(AddNewCardRequestModel taskCardRequest)
        {
            var newCard = _mapper.Map<TaskCard>(taskCardRequest);
            newCard.Id = Guid.NewGuid();
            newCard.Edited = DateTime.Now;

            var isCardAdded = await _writeRepo.Add(newCard);

            if(isCardAdded)
            {
                return newCard;
            }

            return null;
        }

        public async Task SetTaskCardAsCompleted(Guid id)
        {
            var taskCard = await _readRepo.GetById(id);
            taskCard.Completed = true;

            await _writeRepo.Update(taskCard);
        }

        public async Task SetTaskCardTitle(Guid id, string newTitle)
        {
            var taskCard = await _readRepo.GetById(id);

            if(taskCard.Title != newTitle)
            {
                taskCard.Title = newTitle;
                await _writeRepo.Update(taskCard);
            }
        }

        public async Task DeleteTaskCard(Guid id)
        {
            await _writeRepo.Delete(id);
        }
    }
}
