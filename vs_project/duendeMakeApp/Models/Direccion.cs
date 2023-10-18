using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Direccion
{
    public int DireccionId { get; set; }

    public int? CodigoPostal { get; set; }

    public string? Detalle { get; set; }

    public int? ProvinciaId { get; set; }

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual Provincia? Provincia { get; set; }
}
