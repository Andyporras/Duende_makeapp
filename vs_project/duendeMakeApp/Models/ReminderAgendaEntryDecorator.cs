namespace duendeMakeApp.Models
{
    public class ReminderAgendaEntryDecorator : AgendaEntryDecorator
    {
        public string ReminderMessage { get; set; }

        public ReminderAgendaEntryDecorator(IAgendaEntry decoratedEntry, string reminderMessage)
            : base(decoratedEntry)
        {
            ReminderMessage = reminderMessage;
        }
    }

}
