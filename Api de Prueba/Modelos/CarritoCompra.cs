namespace Api_de_Prueba.Modelos
{
    public class CarritoCompra
    {
        int numeroCompra { get; set; }
        string nombreUsuario { get; set; } = string.Empty;
        string nombreProducto { get; set; } = string.Empty;
        int cantidad { get; set; }
        int precioTotal { get; set; }


    }
}
