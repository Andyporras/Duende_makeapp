using System;
using System.Collections.Generic;

namespace duendeMakeApp.Models;

public partial class TipoUsuario
{
    public int TipoUsarioId { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
