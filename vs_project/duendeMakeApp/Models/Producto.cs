using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public int? Cantidad { get; set; }

    public int? CategoriaId { get; set; }

    public bool? Estado { get; set; }

    public int? ImagenId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Imagen? Imagen { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Catalogo> Catalogos { get; set; } = new List<Catalogo>();

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();

    public virtual ICollection<Subcategoria> Subcategoria { get; set; } = new List<Subcategoria>();
}
