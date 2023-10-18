using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Provincia
{
    public int ProvinciaId { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();
}
