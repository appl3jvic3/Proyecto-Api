using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos.DTO
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string correo { get; set; }

        [JsonPropertyName("password")]
        public string contrasena { get; set; }
    }
}
