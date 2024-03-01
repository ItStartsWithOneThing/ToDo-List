
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ToDo_List.Controllers.Extensions;
using ToDo_List.Models.Services;

namespace ToDo_List.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskCardService _taskCardService;

        public HomeController(
            ILogger<HomeController> logger,
            ITaskCardService taskCardService)
        {
            _logger = logger;
            _taskCardService = taskCardService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.GetUserId();
            var allCards = await _taskCardService.GetAllTaskCards(userId);

            if(allCards == null || allCards.Any() == false)
            {
                return View();
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            ViewBag.AllCards = JsonSerializer.Serialize(allCards, options);

            return View();
        }

        [Authorize(Policy = "UnauthenticatedPolicy")]
        public async Task<IActionResult> LogIn()
        {
            return View();
        }

        [Authorize(Policy = "UnauthenticatedPolicy")]
        public async Task<IActionResult> Register()
        {
            return View();
        }
    }
}
