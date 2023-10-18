using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Paquete
{
    public int PaqueteId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? CantidadDisponible { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Catalogo> Catalogos { get; set; } = new List<Catalogo>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
