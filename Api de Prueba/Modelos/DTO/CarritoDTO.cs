namespace Api_de_Prueba.Modelos.DTO
{
    public class CarritoDTO
    {
        public int numeroCompra { get; set; } // opcional al crear (se genera en BD)
        public string nombreUsuario { get; set; } = string.Empty;
        public string nombreProducto { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public int precioTotal { get; set; }
    }
}
