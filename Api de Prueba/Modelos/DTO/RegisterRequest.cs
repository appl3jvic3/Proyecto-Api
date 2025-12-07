using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos.DTO
{
    public class RegisterRequest
    {
        [JsonPropertyName("nombreUsuario")]
        public string nombreUsuario { get; set; }

        [JsonPropertyName("correo")]
        public string correo { get; set; }

        [JsonPropertyName("contrasena")]
        public string contrasena { get; set; }
    }
}
