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

        public IActionResult AgregarEntrada(string detalle, int duracion,DateTime fecha)
        {
            // Agregar la entrada a la base de datos
            using (SqlConnection conexion = new SqlConnection(concStr))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Agenda (UsuarioID, Detalle, FechaInicio, DuracionHoras, TipoEntrada) VALUES (@UsuarioID, @Detalle, @FechaInicio, @DuracionHoras, @TipoEntrada)", conexion);
                cmd.Parameters.AddWithValue("@UsuarioID", _usuario?.UsuarioId);
                cmd.Parameters.AddWithValue("@Detalle", detalle);
                cmd.Parameters.AddWithValue("@FechaInicio", fecha);
                cmd.Parameters.AddWithValue("@DuracionHoras", duracion);
                cmd.Parameters.AddWithValue("@TipoEntrada", "entregar pedido");
                cmd.ExecuteNonQuery();
            }
            // Redireccionar o devolver una vista según sea necesario
            return RedirectToAction("Index");
        }

        public List<IAgendaEntry> GetAgendas ()
        {
            List<IAgendaEntry> agendaEntries = new List<IAgendaEntry>();

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
                        AgendaEntry agenda = new AgendaEntry()
                        {
                            AgendaID = (int)dr["AgendaID"],
                            UsuarioID = (int)dr["UsuarioID"],
                            Detalle = (string)dr["Detalle"],
                            FechaInicio = fechaInicio,
                            DuracionHoras = (int)dr["DuracionHoras"]
                        };
                        if ((string)dr["TipoEntrada"] == "entregar pedido")
                        {
                            agendaEntries.Add(new EntregarPedidoDecorator(agenda));
                        }
                        if ((string)dr["TipoEntrada"] == "revisar inventario")
                        {
                            agendaEntries.Add(new RevisarInventarioDecorator(agenda));
                        }
                    }
                }
            }

            return agendaEntries;
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
                        AgendaEntry agenda = new AgendaEntry()
                        {
                            AgendaID = (int)dr["AgendaID"],
                            //UsuarioID = (int)dr["UsuarioID"],
                            Detalle = (string)dr["Detalle"],
                            FechaInicio = fechaInicio,
                            DuracionHoras = (int)dr["DuracionHoras"]
                        };
                        if ((string)dr["TipoEntrada"] == "entregar pedido")
                        {
                            agendaEvents.Add(new EntregarPedidoDecorator(agenda));
                        }
                        if ((string)dr["TipoEntrada"] == "revisar inventario")
                        {
                            agendaEvents.Add(new RevisarInventarioDecorator(agenda));
                        }
                    }
                }
            }

            return formatoJsonEvent(agendaEvents);
        }

        public ActionResult formatoJsonEvent(List<IAgendaEntry> agendaEvents)
        {
            List<AgendaEvent> agendaEvent = new List<AgendaEvent>();

            foreach (var item in agendaEvents)
            {
                if (item is IAgendaDecorator agendaDecorator)
                {
                    agendaEvent.Add(new AgendaEvent()
                    {
                        id = item.AgendaID ?? 0,
                        title = agendaDecorator.DecoratorType,
                        detalle = item.Detalle,
                        start = item.FechaInicio,
                        end = item.FechaInicio?.AddHours(item.DuracionHoras ?? 0),
                        allDay = false
                    });
                }
                else
                {
                    
                }
            }

            return Json(agendaEvent);
        }



        private class AgendaEvent
        {
            public int? id { get; set; }
            public string? title { get; set; }
            public string? detalle { get; set; }
            public DateTime? start { get; set; }
            public DateTime? end { get; set; }
            public bool? allDay { get; set; }   
        }
    }
}
