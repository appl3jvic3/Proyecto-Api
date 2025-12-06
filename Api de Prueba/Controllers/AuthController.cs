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
                    usuarioId = usuario.usuarioId,
                    nombreUsuario = usuario.nombreUsuario,
                    correo = usuario.correo
                }
            });
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            
            var correoNormalizado = request.correo?.Trim().ToLower();

            // Verificar si ya existe
            if (await _context.Usuario.AnyAsync(u => u.correo.ToLower() == correoNormalizado))
            {
                return BadRequest(new { message = "El correo ya está registrado" });
            }

            var nuevoUsuario = new Usuario
            {
                nombreUsuario = request.nombreUsuario.Trim(),
                correo = correoNormalizado,  
                contrasena = request.contrasena.Trim(),
                celular = null  
            };

            _context.Usuario.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario registrado exitosamente",
                user = new
                {
                    usuarioId = nuevoUsuario.usuarioId,
                    nombreUsuario = nuevoUsuario.nombreUsuario,
                    correo = nuevoUsuario.correo
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
