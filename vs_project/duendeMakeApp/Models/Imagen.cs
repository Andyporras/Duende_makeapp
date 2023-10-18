using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Imagen
{
    public int ImagenId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual ICollection<Maquillaje> Maquillajes { get; set; } = new List<Maquillaje>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
