namespace duendeMakeApp.Models;

public partial class Usuario : IObservable
{
    //public static String SeccionActual { get; set; }

    public int UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Usuario1 { get; set; }

    public string? Clave { get; set; }

    public int? TipoId { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual TipoUsuario? Tipo { get; set; }

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public ICollection<AgendaEntry> Agenda { get; set; }

    public async void Notificar(DuendeappContext context, string titulo, string mensaje)
    {
        Console.WriteLine("Agregando la nueva notificacion\n\n\n\n");
        Notificacion notificacion = new Notificacion();
        notificacion.Titulo = titulo;
        notificacion.Mensaje = mensaje;
        notificacion.UsuarioId = UsuarioId;
        notificacion.FechaEnvio = DateTime.Now;
        notificacion.Visto = false;
        context.Notificaciones.Add(notificacion);
        Notificaciones.Add(notificacion);
        await context.SaveChangesAsync();
    }
}
