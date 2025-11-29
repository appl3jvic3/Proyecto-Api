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

        public class ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: api/Productos
        [HttpGet]
        public async Task<IActionResult> GetAll() { 
        var productos = await _context.Set<Producto>()
                .asNoTracking()
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

    }
}
