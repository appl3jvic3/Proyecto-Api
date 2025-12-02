using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos.DTO
{
    public class RegisterRequest
    {
        [JsonPropertyName("name")]
        public string nombreUsuario { get; set; }

        [JsonPropertyName("email")]
        public string correo { get; set; }

        [JsonPropertyName("password")]
        public string contrasena { get; set; }
    }
}
