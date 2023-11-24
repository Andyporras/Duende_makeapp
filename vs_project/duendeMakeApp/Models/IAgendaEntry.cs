namespace duendeMakeApp.Models
{
    public interface IAgendaEntry
    {
        int? AgendaID { get; set; }
        int? UsuarioID { get; set; }
        string? Detalle { get; set; }
        DateTime? FechaInicio { get; set; }
        int? DuracionHoras { get; set; }
    }
}


