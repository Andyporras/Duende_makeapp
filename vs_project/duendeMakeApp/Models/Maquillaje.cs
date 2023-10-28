using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace duendeMakeApp.Models;

public partial class Maquillaje
{
    public int MaquillajeId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();
}
