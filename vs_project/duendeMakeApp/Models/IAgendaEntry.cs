namespace duendeMakeApp.Models
{
    public interface IAgendaEntry
    {
        string Subject { get; }
        DateTime StartTime { get; }
        int Duration { get; }
    }

}
