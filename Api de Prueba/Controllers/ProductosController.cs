using Microsoft.AspNetCore.Mvc;
using Api_de_Prueba.Data;
using Api_de_Prueba.Modelos;
using Api_de_Prueba.Modelos.DTO;
using Microsoft.EntityFrameworkCore;

namespace Api_de_Prueba.Controllers
{
    [ApiController]
        [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor correcto (inyección de dependencias)
        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: api/Productos
        [HttpGet]
        public async Task<IActionResult> GetAll() { 
        var productos = await _context.Set<Producto>()
                .AsNoTracking()
                .Select(p => new ProductoDTO
                {
                    productoId = p.productoId,
                    nombreProducto = p.nombreProducto,
                    descripcion = p.descripcion,
                    precio = p.precio,
                    cantidadDisponible = p.cantidadDisponible,
                    categoria = p.categoria,
                    descuento = p.descuento
                })
                .ToListAsync();
            return Ok(productos);
        }

        // GET: api/productos/{productoId}
        [HttpGet("{productoId:int}")]
        public async Task<IActionResult> Get(int productoId)
        {
            var p = await _context.Set<Producto>().FindAsync(productoId);
            if (p == null) return NotFound();

            var productoDto = new ProductoDTO
            {
                productoId = p.productoId,
                nombreProducto = p.nombreProducto,
                descripcion = p.descripcion,
                precio = p.precio,
                cantidadDisponible = p.cantidadDisponible,
                categoria = p.categoria,
                descuento = p.descuento
            };

            return Ok(productoDto);
        }

        [HttpGet]
        public IActionResult ObtenerProductos()
        {
            var productos = _context.Producto.ToList();

            // Generar la ruta de imagen para cada producto
            foreach (var producto in productos)
            {
                // Ejemplo: "Charizard VMAX" → "img/productos/charizard-vmax.png"
                var nombreArchivo = producto.nombreProducto
                    .ToLower()
                    .Replace(" ", "-")
                    .Replace("á", "a")
                    .Replace("é", "e")
                    .Replace("í", "i")
                    .Replace("ó", "o")
                    .Replace("ú", "u");

                producto.imagen = $"img/productos/{nombreArchivo}.png";
            }

            return Ok(productos);
        }

        // PUT: api/productos/{productoId}
        [HttpPut("{productoId:int}")]
        public async Task<IActionResult> Update(int productoId, [FromBody] ProductoDTO productoDto)
        {
            if (productoId != productoDto.productoId) return BadRequest("productoId mismatch");

            var producto = await _context.Set<Producto>().FindAsync(productoId);
            if (producto == null) return NotFound();

            producto.nombreProducto = productoDto.nombreProducto;
            producto.descripcion = productoDto.descripcion ?? string.Empty;
            producto.precio = productoDto.precio;
            producto.cantidadDisponible = productoDto.cantidadDisponible;
            producto.categoria = productoDto.categoria;
            producto.descuento = productoDto.descuento;

            _context.Set<Producto>().Update(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/productos/{productoId}
        [HttpDelete("{productoId:int}")]
        public async Task<IActionResult> Delete(int productoId)
        {
            var producto = await _context.Set<Producto>().FindAsync(productoId);
            if (producto == null) return NotFound();

            _context.Set<Producto>().Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
