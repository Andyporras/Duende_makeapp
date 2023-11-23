namespace duendeMakeApp.Models
{
    public interface IAgendaEntry
    {
        int? AgendaID { get; set; }
        int? UsuarioID { get; set; }
        string? Asunto { get; set; }
        DateTime? FechaInicio { get; set; }
        int? DuracionMinutos { get; set; }
    }
}

