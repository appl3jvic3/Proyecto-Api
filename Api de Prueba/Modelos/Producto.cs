namespace Api_de_Prueba.Modelos
{
    public class Producto
    {
     
        public int productoId { get; set; }
       
        public String nombreProducto { get; set; }
        public String descripcion { get; set; }
        public double precio { get; set; }
        public int cantidadDisponible { get; set; }
        public String categoria { get; set; }
        public double descuento { get; set; }
    }
}
