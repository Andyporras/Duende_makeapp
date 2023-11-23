using System;
using Microsoft.EntityFrameworkCore;

namespace duendeMakeApp.Models
{
    public class AgendaEntry : IAgendaEntry
    {
        public int? AgendaID { get; set; }
        public int? UsuarioID { get; set; }
        public string? Asunto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public int? DuracionMinutos { get; set; }
        public string? TipoEntrada { get; set; }

        // Relación con la tabla Usuario
        public virtual Usuario Usuario { get; set; }

    }
}
