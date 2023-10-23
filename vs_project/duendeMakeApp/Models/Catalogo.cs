using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace duendeMakeApp.Models;

public partial class Catalogo
{
    public int CatalogoId { get; set; }

    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El campo Descripcion es obligatorio.")]
    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Paquete> Paquetes { get; set; } = new List<Paquete>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
