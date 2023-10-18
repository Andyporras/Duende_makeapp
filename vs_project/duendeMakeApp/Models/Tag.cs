using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();
}
