using duendeMakeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace duendeMakeApp.Controllers
{
    public class AgendasController : Controller
    {
        private readonly DuendeappContext _context;
        private List<Producto> productosCarrito = new List<Producto>();
        private static Usuario? _usuario;

        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private static string conStr;// = "Data Source=DESKTOP-993UODJ; Initial Catalog=DUENDEAPP; Integrated Security=true; Encrypt=False;";
        private static string? conStr;

        public AgendasController(DuendeappContext context, Usuario usuario, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _usuario = usuario;
            _context = context;
            _clientFactory = clientFactory;
            var serverName = _context.Database.GetDbConnection().DataSource;
            var databaseName = _context.Database.GetDbConnection().Database;
            var integratedSecurity = _context.Database.GetDbConnection().ConnectionString.Contains("Integrated Security=true");
            var encrypt = _context.Database.GetDbConnection().ConnectionString.Contains("Encrypt=true");
            var user = _context.Database.GetDbConnection().ConnectionString.Contains("User=");
            var password = _context.Database.GetDbConnection().ConnectionString.Contains("Password=");
            var conStrBuilder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                InitialCatalog = databaseName,
                IntegratedSecurity = integratedSecurity,
                Encrypt = encrypt,
                UserID = user ? _context.Database.GetDbConnection().ConnectionString.Split("User=")[1].Split(";")[0] : "",
                Password = password ? _context.Database.GetDbConnection().ConnectionString.Split("Password=")[1].Split(";")[0] : ""
            };
            conStr = conStrBuilder.ConnectionString;
            Console.WriteLine(conStr);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
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
