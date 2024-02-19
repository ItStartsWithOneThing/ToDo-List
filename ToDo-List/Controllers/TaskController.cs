
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo_List.Models.API.Requests;
using ToDo_List.Models.API.Responses;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DTO;
using ToDo_List.Models.Services;

namespace ToDo_List.Controllers
{
    [Authorize]
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


        /// <summary>
        /// Create new task card
        /// </summary>
        /// <response code="200"></response>
        /// <response code="400">Failed to add new card</response>
        [HttpPost("add-card")]
        [ProducesResponseType(typeof(TaskCard), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCard([FromBody] AddNewCardRequestModel request)
        {
            var result = await _taskCardService.AddTaskCard(request);
            return result != null ? Ok(result) : BadRequest("Failed to add new card");
        }

        /// <summary>
        /// Update cards with unsaved changes
        /// </summary>
        /// <response code="200">Updated 5 cards</response>
        /// <response code="400">Failed to update 5 cards</response>
        [HttpPost("update-cards")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCards([FromBody] IEnumerable<TaskCardDto> request)
        {
            var result = await _taskCardService.UpdateTaskCards(request);
            return result == true ? Ok($"Updated {request.Count()} cards") : BadRequest($"Failed to update {request.Count()} cards");
        }

        /// <summary>
        /// Delete card
        /// </summary>
        /// <response code="200">Deleted card with id: 3fa85f64-5717-4562-b3fc-2c963f66afa6</response>
        /// <response code="400">Failed to delete card with id: 3fa85f64-5717-4562-b3fc-2c963f66afa6</response>
        [HttpDelete("delete-card")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCard([FromQuery] Guid id)
        {
            var result = await _taskCardService.DeleteTaskCard(id);
            return result == true ? Ok($"Deleted card with id: {id}") : BadRequest($"Failed to delete card with id: {id}");
        }
    }
}
