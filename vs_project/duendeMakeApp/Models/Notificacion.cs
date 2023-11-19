using duendeMakeApp.Models;
namespace duendeMakeApp.Models;

public partial class Notificacion
{
    public int NotificacionId { get; set; }
    public string Titulo { get; set; }
    public string Mensaje { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaEnvio { get; set; }
    public bool Visto { get; set; }

    public virtual Usuario Usuario { get; set; }
}
