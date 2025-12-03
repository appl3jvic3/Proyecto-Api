using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos
{
    public class CarritoCompra
    {
        [Key]
        [JsonPropertyName("id")]
        public int numeroCompra { get; set; }

        // Si decides mantener nombres (no recomendado)
        [JsonPropertyName("userName")]
        public string nombreUsuario { get; set; } = string.Empty;

        [JsonPropertyName("productName")]
        public string nombreProducto { get; set; } = string.Empty;

        // O mejor, usar IDs (recomendado)
        [JsonPropertyName("userId")]
        public int usuarioId { get; set; }

         [JsonPropertyName("productId")]
         public int productoId { get; set; }

        [JsonPropertyName("quantity")]
        public int cantidad { get; set; }

        [JsonPropertyName("totalPrice")]
        public decimal precioTotal { get; set; }

    }
}
