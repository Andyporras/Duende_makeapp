namespace duendeMakeApp.Models
{
    public class ColoredAgendaEntryDecorator : AgendaEntryDecorator
    {
        public string Color { get; set; }

        public ColoredAgendaEntryDecorator(IAgendaEntry decoratedEntry, string color)
            : base(decoratedEntry)
        {
            Color = color;
        }
    }

}
