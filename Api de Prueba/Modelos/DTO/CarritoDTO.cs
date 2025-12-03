using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos.DTO
{
    public class CarritoDTO
    {
        [JsonPropertyName("id")]
        public int numeroCompra { get; set; }

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
