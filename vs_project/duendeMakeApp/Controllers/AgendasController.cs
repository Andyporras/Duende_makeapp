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
        private static string? concStr;

        private List<IAgendaEntry> agendaEntries = new List<IAgendaEntry>();

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
            concStr = conStrBuilder.ConnectionString;
            Console.WriteLine(concStr);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            ViewBag.Usuario = UsuariosController.GetSessionUser(_httpContextAccessor, _context);
            ViewBag.AgendaEntries = new List<AgendaEntry>();  // O carga las entradas reales aquí
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

        public IActionResult AgregarEntrada(string asunto, int duracion,DateTime date)
        {
           
            // Redireccionar o devolver una vista según sea necesario
            return RedirectToAction("Index");
        }
    
        public ActionResult GetAgendaEvents()
        {
            List<IAgendaEntry> agendaEvents = new List<IAgendaEntry>();

            using (SqlConnection conexion = new SqlConnection(concStr))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Agenda", conexion);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DateTime? fechaInicio = null;

                        try
                        {
                            if (dr["FechaInicio"] != DBNull.Value)
                            {
                                fechaInicio = (DateTime?)dr["FechaInicio"];
                            }
                        }
                        catch (InvalidCastException)
                        {
                            fechaInicio = null;
                        }

                        agendaEvents.Add(new AgendaEntry()
                        {
                            AgendaID = (int)dr["AgendaID"],
                            UsuarioID = (int)dr["UsuarioID"],
                            Asunto = (string)dr["Asunto"],
                            FechaInicio = fechaInicio,
                            DuracionMinutos = (int)dr["DuracionMinutos"]
                            // Agrega otras propiedades según sea necesario
                        });
                    }
                }
            }

            //return Json(agendaEvents);
            //var jsonData = new { Events = agendaEvents, PartialView = PartialView("_AgendaPartialView", agendaEvents).ToString() };
            // cambiar el formato 
            return formatoJsonEvent(agendaEvents);
        }

        public ActionResult formatoJsonEvent(List<IAgendaEntry> agendaEvents)
        {
            List<AgendaEvent> agendaEvent = new List<AgendaEvent>();
            foreach (var item in agendaEvents)
            {
                agendaEvent.Add(new AgendaEvent()
                {
                    id = item.AgendaID,
                    title = item.Asunto,
                    start = item.FechaInicio,
                    end = item.FechaInicio?.AddMinutes(item.DuracionMinutos ?? 0),
                    allDay = false
                });
            }
            return Json(agendaEvent);
        }

        private class AgendaEvent
        {
            public int? id { get; set; }
            public string? title { get; set; }
            public DateTime? start { get; set; }
            public DateTime? end { get; set; }
            public bool? allDay { get; set; }   
        }
    }
}
