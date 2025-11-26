using System.ComponentModel.DataAnnotations;

namespace Api_de_Prueba.Modelos
{
    public class Usuario
    {
        [Key]
        public int usuarioId { get; set; }


        [Required]
        [MaxLength(100)]
        public string nombreUsuario { get; set; }


        [MaxLength(200)]
        public string direccion { get; set; }

        public int celular { get; set; }


        [Required]
        [EmailAddress]
        public string correo { get; set; }

        [Required]
        public string contrasena { get; set; }
    }
}
