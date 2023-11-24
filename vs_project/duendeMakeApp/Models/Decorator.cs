namespace duendeMakeApp.Models
{

    public interface IAgendaDecorator : IAgendaEntry
    {
        string DecoratorType { get; set; }
    }

    public class MaquillajesDecorator : IAgendaDecorator
    {
        private readonly IAgendaEntry _agendaEntry;

        public MaquillajesDecorator(IAgendaEntry agendaEntry)
        {
            _agendaEntry = agendaEntry;
            DecoratorType = "Maquillajes";
        }

        public int? AgendaID
        {
            get => _agendaEntry.AgendaID;
            set => _agendaEntry.AgendaID = value;
        }

        public int? UsuarioID
        {
            get => _agendaEntry.UsuarioID;
            set => _agendaEntry.UsuarioID = value;
        }

        public string? Detalle
        {
            get => _agendaEntry.Detalle;
            set => _agendaEntry.Detalle = value;
        }

        public DateTime? FechaInicio
        {
            get => _agendaEntry.FechaInicio;
            set => _agendaEntry.FechaInicio = value;
        }

        public int? DuracionHoras
        {
            get => _agendaEntry.DuracionHoras;
            set => _agendaEntry.DuracionHoras = value;
        }

        public string DecoratorType { get; set; }
    }


    public class EntregarPedidoDecorator : IAgendaDecorator
    {
        private readonly IAgendaEntry _agendaEntry;

        public EntregarPedidoDecorator(IAgendaEntry agendaEntry)
        {
            _agendaEntry = agendaEntry;
            DecoratorType = "Entregar Pedido";
        }

        public int? AgendaID
        {
            get => _agendaEntry.AgendaID;
            set => _agendaEntry.AgendaID = value;
        }

        public int? UsuarioID
        {
            get => _agendaEntry.UsuarioID;
            set => _agendaEntry.UsuarioID = value;
        }

        public string? Detalle
        {
            get => _agendaEntry.Detalle;
            set => _agendaEntry.Detalle = value;
        }

        public DateTime? FechaInicio
        {
            get => _agendaEntry.FechaInicio;
            set => _agendaEntry.FechaInicio = value;
        }

        public int? DuracionHoras
        {
            get => _agendaEntry.DuracionHoras;
            set => _agendaEntry.DuracionHoras = value;
        }


        public string DecoratorType { get; set; }
    }

    public class RevisarInventarioDecorator : IAgendaDecorator
    {
        private readonly IAgendaEntry _agendaEntry;

        public RevisarInventarioDecorator(IAgendaEntry agendaEntry)
        {
            _agendaEntry = agendaEntry;
            DecoratorType = "Revisar Inventario";
        }

        public int? AgendaID
        {
            get => _agendaEntry.AgendaID;
            set => _agendaEntry.AgendaID = value;
        }

        public int? UsuarioID
        {
            get => _agendaEntry.UsuarioID;
            set => _agendaEntry.UsuarioID = value;
        }

        public string? Detalle
        {
            get => _agendaEntry.Detalle;
            set => _agendaEntry.Detalle = value;
        }

        public DateTime? FechaInicio
        {
            get => _agendaEntry.FechaInicio;
            set => _agendaEntry.FechaInicio = value;
        }

        public int? DuracionHoras
        {
            get => _agendaEntry.DuracionHoras;
            set => _agendaEntry.DuracionHoras = value;
        }


        public string DecoratorType { get; set; }
    }
}
