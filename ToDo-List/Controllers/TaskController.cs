
using Microsoft.AspNetCore.Mvc;
using ToDo_List.Models.Requests;
using ToDo_List.Models.Services;

namespace ToDo_List.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskCardService _taskCardService;

        public TaskController(
            ILogger<TaskController> logger,
            ITaskCardService taskCardService)
        {
            _logger = logger;
            _taskCardService = taskCardService;
        }


        [HttpPost("add-card")]
        public async Task<IActionResult> AddCard(AddNewCardRequestModel request)
        {
            var result = await _taskCardService.AddTaskCard(request);
            return result == null ? BadRequest() : Ok(result);
        }
    }
}
