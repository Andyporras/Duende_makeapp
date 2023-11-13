using duendeMakeApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace duendeMakeApp.Controllers
{
    public class AgendasController : Controller
    {
        private readonly ILogger<AgendasController> _logger;

        public AgendasController(ILogger<AgendasController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
