using duendeMakeApp.DAO;
using duendeMakeApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace duendeMakeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = EmailSenderDAO.GetInstance();
        }

        public async Task<IActionResult>Index()
        {
            await _emailSender.SendEmailAsync("miltonjosue2001@gmail.com", "duendeMakeApp", "Hola, \n Tu código es 123497yny4.\n\nSaludos,\n\nDuende Milton😀.");
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