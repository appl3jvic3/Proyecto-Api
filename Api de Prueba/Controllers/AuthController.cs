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
            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.correo == request.email);

            if (usuario == null || usuario.contrasena != request.password)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
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
            if (await _context.Usuario.AnyAsync(u => u.correo == request.email))
            {
                return BadRequest(new { message = "El email ya está registrado" });
            }

            var nuevoUsuario = new Usuario
            {
                nombreUsuario = request.name,
                correo = request.email,
                contrasena = request.password, // En producción, usa hash
                
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
