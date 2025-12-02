using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
namespace Api_de_Prueba.Modelos
{
    public class Producto
{
    [JsonPropertyName("id")]
    public int productoId { get; set; }

    [JsonPropertyName("name")]
    public string nombreProducto { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string descripcion { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public decimal precio { get; set; }

    [JsonPropertyName("stock")]
    public int cantidadDisponible { get; set; }

    [JsonPropertyName("category")]
    public string categoria { get; set; } = string.Empty;

    [JsonPropertyName("discount")]
    public double descuento { get; set; }

    // Esta propiedad NO se guarda en la base de datos
    [NotMapped]  // ⭐ IMPORTANTE: Le dice a Entity Framework que ignore esta propiedad
    [JsonPropertyName("image")]
    public string imagen { get; set; } = string.Empty;
}
}
