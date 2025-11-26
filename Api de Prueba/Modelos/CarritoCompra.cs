namespace Api_de_Prueba.Modelos
{
    public class CarritoCompra
    {
        public int numeroCompra { get; set; }
        public string nombreUsuario { get; set; } = string.Empty;
        public string nombreProducto { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public int precioTotal { get; set; }


    }
}
