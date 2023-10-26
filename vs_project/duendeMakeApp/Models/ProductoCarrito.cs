namespace duendeMakeApp.Models
{
    public class ProductoCarrito
    {
        public int ProductoId { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public decimal? Precio { get; set; }

        public int? Cantidad { get; set; }

        public int? CategoriaId { get; set; }

        public bool? Estado { get; set; }

        public int? ImagenId { get; set; }

        public string? url { get; set; }
    }
}
