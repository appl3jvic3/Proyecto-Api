using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos.DTO
{
    public class OrderDto
    {
        [JsonPropertyName("orderId")]
        public int numeroCompra { get; set; }

        [JsonPropertyName("userId")]
        public int usuarioId { get; set; }

        [JsonPropertyName("date")]
        public DateTime fechaCompra { get; set; }

        [JsonPropertyName("total")]
        public decimal totalAmount { get; set; }

        [JsonPropertyName("itemsCount")]
        public int itemsCount { get; set; }
    }
}
