using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos.DTO
{
    public class CarritoDTO
    {
     

        [JsonPropertyName("id")]
        public int? numeroCompra { get; set; }

        [JsonPropertyName("usuarioId")]  // ✅ Cambio: de "userId" a "usuarioId"
        public int usuarioId { get; set; }

        [JsonPropertyName("productoId")]  // ✅ Cambio: de "productId" a "productoId"
        public int productoId { get; set; }

        [JsonPropertyName("cantidad")]  // ✅ Cambio: de "quantity" a "cantidad"
        public int cantidad { get; set; }

        [JsonPropertyName("precioTotal")]  // ✅ Cambio: de "totalPrice" a "precioTotal"
        public decimal precioTotal { get; set; }
    }
}