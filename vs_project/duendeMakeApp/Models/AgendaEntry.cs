using System;
using Microsoft.EntityFrameworkCore;

namespace duendeMakeApp.Models
{
    public class AgendaEntry : IAgendaEntry
    {
        public int? AgendaID { get; set; }
        public int? UsuarioID { get; set; }
        public string? Detalle { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? DuracionHoras { get; set; }

        // Relación con la tabla Usuario
        public virtual Usuario Usuario { get; set; }

    }
}
