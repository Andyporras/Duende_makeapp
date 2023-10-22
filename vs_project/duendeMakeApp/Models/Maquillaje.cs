using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace duendeMakeApp.Models;

public partial class Maquillaje
{
    public int MaquillajeId { get; set; }

    [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El campo Descripcion es obligatorio.")]
    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    [Required(ErrorMessage = "Es necesario agregar al menos una imagen.")]
    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();
}
