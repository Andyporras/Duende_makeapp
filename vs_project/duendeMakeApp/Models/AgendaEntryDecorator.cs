namespace duendeMakeApp.Models
{
    public abstract class AgendaEntryDecorator : IAgendaEntry
    {
        private readonly IAgendaEntry _decoratedEntry;

        public AgendaEntryDecorator(IAgendaEntry decoratedEntry)
        {
            _decoratedEntry = decoratedEntry;
        }

        public virtual string Subject => _decoratedEntry.Subject;
        public virtual DateTime StartTime => _decoratedEntry.StartTime;
        public virtual int Duration => _decoratedEntry.Duration;
    }

}
