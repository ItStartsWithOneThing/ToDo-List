
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Repositories;

namespace ToDo_List.Models.Services
{
    public class TaskCardService : ITaskCardService
    {
        private readonly IReadRepository _readRepo;
        private readonly IWriteRepository _writeRepo;

        public TaskCardService(
            IReadRepository readRepo,
            IWriteRepository writeRepo)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public async Task<IEnumerable<TaskCard>> GetAllTaskCards()
        {
            return await _readRepo.GetAll();
        }

        public async Task AddTaskCard(TaskCard taskCard)
        {
            taskCard.Id = Guid.NewGuid();
            taskCard.CreatedDate = DateTime.Now;

            await _writeRepo.Add(taskCard);
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
