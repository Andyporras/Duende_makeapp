using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Carrito
{
    public int CarritoId { get; set; }

    public int? UsuarioId { get; set; }

    public bool estado { get; set;}

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual Usuario? Usuario { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
