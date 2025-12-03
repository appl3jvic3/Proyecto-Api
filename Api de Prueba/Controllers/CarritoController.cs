using Api_de_Prueba.Data;
using Api_de_Prueba.Modelos;
using Api_de_Prueba.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_de_Prueba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Carrito/{numeroCompra}
        [HttpGet("{numeroCompra:int}")]
        public async Task<IActionResult> GetByNumeroCompra(int numeroCompra)
        {
            var carrito = await _context.CarritoCompra
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.numeroCompra == numeroCompra);

            if (carrito == null) return NotFound();

            return Ok(carrito);
        }

        // GET: api/Carrito/user/{usuarioId}
        [HttpGet("user/{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var carritoItems = await _context.CarritoCompra
                .Include(c => c.Producto)
                .AsNoTracking()
                .Where(c => c.usuarioId == usuarioId)
                .ToListAsync();

            return Ok(carritoItems);
        }

        // POST: api/Carrito
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarritoDTO carritoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Verificar que usuario y producto existan
            var usuarioExiste = await _context.Usuario.AnyAsync(u => u.usuarioId == carritoDto.usuarioId);
            if (!usuarioExiste)
                return BadRequest(new { message = "Usuario no encontrado" });

            var productoExiste = await _context.Producto.AnyAsync(p => p.productoId == carritoDto.productoId);
            if (!productoExiste)
                return BadRequest(new { message = "Producto no encontrado" });

            var carrito = new CarritoCompra
            {
                usuarioId = carritoDto.usuarioId,
                productoId = carritoDto.productoId,
                cantidad = carritoDto.cantidad,
                precioTotal = carritoDto.precioTotal
            };

            _context.CarritoCompra.Add(carrito);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByNumeroCompra),
                new { numeroCompra = carrito.numeroCompra }, carrito);
        }

        // PUT: api/Carrito/{numeroCompra}
        [HttpPut("{numeroCompra:int}")]
        public async Task<IActionResult> Update(int numeroCompra, [FromBody] CarritoDTO carritoDto)
        {
            if (numeroCompra != carritoDto.numeroCompra)
                return BadRequest("numeroCompra mismatch");

            var carrito = await _context.CarritoCompra.FindAsync(numeroCompra);
            if (carrito == null) return NotFound();

            carrito.usuarioId = carritoDto.usuarioId;
            carrito.productoId = carritoDto.productoId;
            carrito.cantidad = carritoDto.cantidad;
            carrito.precioTotal = carritoDto.precioTotal;

            _context.CarritoCompra.Update(carrito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Carrito/{numeroCompra}
        [HttpDelete("{numeroCompra:int}")]
        public async Task<IActionResult> Delete(int numeroCompra)
        {
            var carrito = await _context.CarritoCompra.FindAsync(numeroCompra);
            if (carrito == null) return NotFound();

            _context.CarritoCompra.Remove(carrito);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}