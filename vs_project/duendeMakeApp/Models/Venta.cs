using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public int? ImgComprobante { get; set; }

    public int? CarritoId { get; set; }

    public virtual Carrito? Carrito { get; set; }

    public virtual Imagen? ImgComprobanteNavigation { get; set; }
}
