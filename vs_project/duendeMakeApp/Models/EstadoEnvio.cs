using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class EstadoEnvio
{
    public int EstadoId { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();
}
