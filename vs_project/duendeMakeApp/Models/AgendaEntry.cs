namespace duendeMakeApp.Models
{
    public class AgendaEntry : IAgendaEntry
    {
        public string Subject { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
    }

}
