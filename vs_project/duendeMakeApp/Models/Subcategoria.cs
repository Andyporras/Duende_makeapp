using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Subcategoria
{
    public int SubcategoriaId { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
