
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using ToDo_List.Models;
using ToDo_List.Models.Services;

namespace ToDo_List.Controllers
{
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
            var allCards = await _taskCardService.GetAllTaskCards();

            if(allCards == null)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
