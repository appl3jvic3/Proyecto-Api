namespace Api_de_Prueba.Modelos.DTO
{
    public class ProductoDTO
    {
        public int productoId { get; set; }
        public string nombreProducto { get; set; } = string.Empty;
        public string? descripcion { get; set; }
        public double precio { get; set; }
        public int cantidadDisponible { get; set; }
        public string categoria { get; set; } = string.Empty;
        public double descuento { get; set; }
    }
}
