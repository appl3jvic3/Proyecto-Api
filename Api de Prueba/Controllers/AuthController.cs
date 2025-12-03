using Api_de_Prueba.Data;
using Api_de_Prueba.Modelos;
using Api_de_Prueba.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_de_Prueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            // Normalizar el correo (quitar espacios y convertir a minúsculas)
            var correoNormalizado = request.correo?.Trim().ToLower();

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.correo.ToLower() == correoNormalizado);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario no encontrado" });
            }

            // Comparar contraseña (quitando espacios)
            if (usuario.contrasena.Trim() != request.contrasena.Trim())
            {
                return Unauthorized(new { message = "Contraseña incorrecta" });
            }

            return Ok(new
            {
                token = GenerarToken(usuario),
                user = new
                {
                    id = usuario.usuarioId,
                    name = usuario.nombreUsuario,
                    email = usuario.correo
                }
            });
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            // Verificar si ya existe
            if (await _context.Usuario.AnyAsync(u => u.correo == request.correo))
            {
                return BadRequest(new { message = "El correo ya está registrado" });
            }

            var nuevoUsuario = new Usuario
            {
                nombreUsuario = request.nombreUsuario,
                correo = request.correo,
                contrasena = request.contrasena, // En producción, usa hash
                
            };

            _context.Usuario.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario registrado exitosamente",
                user = new
                {
                    id = nuevoUsuario.usuarioId,
                    name = nuevoUsuario.nombreUsuario,
                    email = nuevoUsuario.correo
                }
            });
        }

        private string GenerarToken(Usuario usuario)
        {
            // Simplificado para desarrollo
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{usuario.usuarioId}:{usuario.correo}:{DateTime.Now}")
            );
        }
    }

    
}
