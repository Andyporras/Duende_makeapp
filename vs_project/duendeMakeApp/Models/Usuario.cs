using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace duendeMakeApp.Models;

public partial class Usuario
{
    //public static String SeccionActual { get; set; }

    public int UsuarioId { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Usuario1 { get; set; }

    public string? Clave { get; set; }

    public int? TipoId { get; set; }

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual TipoUsuario? Tipo { get; set; }
}
