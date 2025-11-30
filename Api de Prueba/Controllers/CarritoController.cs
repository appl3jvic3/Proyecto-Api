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

        // GET: api/carrito/{numeroCompra}
        [HttpGet("{numeroCompra:int}")]
        public async Task<IActionResult> GetByNumeroCompra(int numeroCompra)
        {
            var carrito = await _context.Set<CarritoCompra>()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.numeroCompra == numeroCompra);

            if (carrito == null) return NotFound();

            var carritoDto = new CarritoDTO
            {
                numeroCompra = carrito.numeroCompra,
                nombreUsuario = carrito.nombreUsuario,
                nombreProducto = carrito.nombreProducto,
                cantidad = carrito.cantidad,
                precioTotal = carrito.precioTotal
            };

            return Ok(carritoDto);
        }

        // GET: api/carrito/user/{nombreUsuario}
        // Lista todos los registros de carrito para un usuario (puede servir como historial)
        [HttpGet("user/{nombreUsuario}")]
        public async Task<IActionResult> GetByUsuario(string nombreUsuario)
        {
            var carritoItems = await _context.Set<CarritoCompra>()
                .AsNoTracking()
                .Where(c => c.nombreUsuario == nombreUsuario)
                .Select(c => new CarritoDTO
                {
                    numeroCompra = c.numeroCompra,
                    nombreUsuario = c.nombreUsuario,
                    nombreProducto = c.nombreProducto,
                    cantidad = c.cantidad,
                    precioTotal = c.precioTotal
                })
                .ToListAsync();

            return Ok(carritoItems);
        }

        // POST: api/carrito
        // Añade un item (registro) al carrito/historial. Si quieres agrupar items por compra necesitaríamos un modelo Order/OrderItem.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarritoDTO carritoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var carrito = new CarritoCompra
            {
                nombreUsuario = carritoDto.nombreUsuario,
                nombreProducto = carritoDto.nombreProducto,
                cantidad = carritoDto.cantidad,
                precioTotal = carritoDto.precioTotal
            };

            _context.Set<CarritoCompra>().Add(carrito);
            await _context.SaveChangesAsync();

            carritoDto.numeroCompra = carrito.numeroCompra;
            return CreatedAtAction(nameof(GetByNumeroCompra), new { numeroCompra = carritoDto.numeroCompra }, carritoDto);
        }

        // PUT: api/carrito/{numeroCompra}
        [HttpPut("{numeroCompra:int}")]
        public async Task<IActionResult> Update(int numeroCompra, [FromBody] CarritoDTO carritoDto)
        {
            if (numeroCompra != carritoDto.numeroCompra) return BadRequest("numeroCompra mismatch");

            var carrito = await _context.Set<CarritoCompra>().FindAsync(numeroCompra);
            if (carrito == null) return NotFound();

            carrito.nombreUsuario = carritoDto.nombreUsuario;
            carrito.nombreProducto = carritoDto.nombreProducto;
            carrito.cantidad = carritoDto.cantidad;
            carrito.precioTotal = carritoDto.precioTotal;

            _context.Set<CarritoCompra>().Update(carrito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/carrito/{numeroCompra}
        [HttpDelete("{numeroCompra:int}")]
        public async Task<IActionResult> Delete(int numeroCompra)
        {
            var carrito = await _context.Set<CarritoCompra>().FindAsync(numeroCompra);
            if (carrito == null) return NotFound();

            _context.Set<CarritoCompra>().Remove(carrito);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}