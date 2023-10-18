using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Envio
{
    public int EnvioId { get; set; }

    public DateTime? FechaPedido { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public int? EstadoId { get; set; }

    public int? CarritoId { get; set; }

    public int? DireccionId { get; set; }

    public virtual Carrito? Carrito { get; set; }

    public virtual Direccion? Direccion { get; set; }

    public virtual EstadoEnvio? Estado { get; set; }
}
