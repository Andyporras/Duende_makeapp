namespace duendeMakeApp.Models
{
    public class Pedido
    {

        public int IdVenta { get; set; }

        public string Cliente { get; set; }
        public decimal? Monto { get; set; }

        public DateTime? FechaPedido { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public string? Direccion { get; set; }

        public int? estado { get; set; }

        public string? imagen { get; set; }
    }
}

