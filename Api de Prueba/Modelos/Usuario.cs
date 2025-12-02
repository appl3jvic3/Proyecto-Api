using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api_de_Prueba.Modelos
{
    public class Usuario
    {
        [Key]
        [JsonPropertyName("id")]
        public int usuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("name")]
        public string nombreUsuario { get; set; } = string.Empty;

        [MaxLength(200)]
        [JsonPropertyName("address")]
        public string? direccion { get; set; }

        [JsonPropertyName("phone")]
        public int celular { get; set; }

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string correo { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("password")]
        public string contrasena { get; set; } = string.Empty;
    }
}
