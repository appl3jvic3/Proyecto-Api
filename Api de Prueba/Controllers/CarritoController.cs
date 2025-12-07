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
        // Cambios hechos por Luis:
        // - Agregado mapeo manual para devolver datos en formato consistente con el frontend
        // - Incluye: numeroCompra, usuarioId, productoId, cantidad, precioTotal, fechaCompra
        [HttpGet("user/{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var carritoItems = await _context.CarritoCompra
                .Include(c => c.Producto)
                .AsNoTracking()
                .Where(c => c.usuarioId == usuarioId)
                .OrderByDescending(c => c.fechaCompra)  // Mas recientes primero
                .ToListAsync();

            // Mapear a un formato consistente
            var resultado = carritoItems.Select(c => new
            {
                numeroCompra = c.numeroCompra,
                usuarioId = c.usuarioId,
                productoId = c.productoId,
                cantidad = c.cantidad,
                precioTotal = c.precioTotal,
                fechaCompra = c.fechaCompra
            });

            return Ok(resultado);
        }

        // POST: api/Carrito
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CarritoDTO carritoDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Verificar que usuario y producto existan
            var usuarioExiste = await _context.Usuario.AnyAsync(u => u.usuarioId == carritoDto.usuarioId);
            if (!usuarioExiste)
                return BadRequest(new { message = $"Usuario con ID {carritoDto.usuarioId} no encontrado en la base de datos" });

            var productoExiste = await _context.Producto.AnyAsync(p => p.productoId == carritoDto.productoId);
            if (!productoExiste)
                return BadRequest(new { message = "Producto no encontrado" });

            var carrito = new CarritoCompra
            {
                usuarioId = carritoDto.usuarioId,
                productoId = carritoDto.productoId,
                cantidad = carritoDto.cantidad,
                precioTotal = carritoDto.precioTotal,
                fechaCompra = DateTime.UtcNow
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

        // POST: api/Carrito/checkout
        // METODO AGREGADO POR LUIS:
        // Procesa la compra final del usuario, validando stock y actualizando inventario
        //
        // Funcionalidad implementada:
        // 1. Valida que el usuario exista en la base de datos
        // 2. Valida que el producto exista en la base de datos
        // 3. Verifica que haya suficiente stock disponible (cantidadDisponible >= cantidad solicitada)
        // 4. Crea el registro de compra en la tabla CarritoCompra
        // 5. Reduce automaticamente el stock del producto en la tabla Productos
        // 6. Guarda ambos cambios en una sola transaccion
        //
        // Flujo de datos:
        // Request body esperado:
        // {
        //   "usuarioId": 1,
        //   "productoId": 1,
        //   "cantidad": 2,
        //   "precioTotal": 240.00
        // }
        //
        // Respuesta exitosa (200 OK):
        // {
        //   "mensaje": "Compra realizada con exito",
        //   "numeroCompra": 5,
        //   "stockRestante": 13
        // }
        //
        // Respuesta de error - Stock insuficiente (400 Bad Request):
        // {
        //   "mensaje": "Stock insuficiente. Solo quedan 1 unidades disponibles.",
        //   "stockDisponible": 1
        // }
        //
        // Respuesta de error - Usuario no encontrado (404 Not Found):
        // {
        //   "mensaje": "Usuario con ID X no encontrado"
        // }
        //
        // Respuesta de error - Producto no encontrado (404 Not Found):
        // {
        //   "mensaje": "Producto con ID X no encontrado"
        // }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CarritoDTO carritoDTO)
        {
            try
            {
                // Paso 1: Validar que el usuario exista
                var usuarioExiste = await _context.Usuario
                    .AnyAsync(u => u.usuarioId == carritoDTO.usuarioId);

                if (!usuarioExiste)
                {
                    return NotFound(new { mensaje = $"Usuario con ID {carritoDTO.usuarioId} no encontrado" });
                }

                // Paso 2: Validar que el producto exista y obtener sus datos completos
                // Necesitamos los datos completos para verificar y actualizar el stock
                var producto = await _context.Producto
                    .FirstOrDefaultAsync(p => p.productoId == carritoDTO.productoId);

                if (producto == null)
                {
                    return NotFound(new { mensaje = $"Producto con ID {carritoDTO.productoId} no encontrado" });
                }

                // Paso 3: VALIDACION DE STOCK AGREGADA POR LUIS
                // Verificar que haya suficiente stock disponible antes de procesar la compra
                // Si la cantidad solicitada es mayor al stock disponible, devolver error 400
                if (producto.cantidadDisponible < carritoDTO.cantidad)
                {
                    return BadRequest(new
                    {
                        mensaje = $"Stock insuficiente. Solo quedan {producto.cantidadDisponible} unidades disponibles.",
                        stockDisponible = producto.cantidadDisponible
                    });
                }

                // Paso 4: Crear el registro de compra en la tabla CarritoCompra
                var carritoCompra = new CarritoCompra
                {
                    usuarioId = carritoDTO.usuarioId,
                    productoId = carritoDTO.productoId,
                    cantidad = carritoDTO.cantidad,
                    precioTotal = carritoDTO.precioTotal,
                    fechaCompra = DateTime.Now
                };

                _context.CarritoCompra.Add(carritoCompra);

                // Paso 5: ACTUALIZACION DE INVENTARIO AGREGADA POR LUIS
                // Reducir la cantidad disponible del producto en la tabla Productos
                // Ejemplo: Si habia 15 unidades y se compraron 2, ahora quedan 13
                producto.cantidadDisponible -= carritoDTO.cantidad;
                _context.Producto.Update(producto);

                // Paso 6: Guardar todos los cambios en la base de datos
                // SaveChangesAsync guarda tanto el registro de compra como la actualizacion del stock
                // en una sola transaccion (si falla uno, se revierte todo)
                await _context.SaveChangesAsync();

                // Devolver respuesta exitosa con informacion util para el frontend
                return Ok(new
                {
                    mensaje = "Compra realizada con exito",
                    numeroCompra = carritoCompra.numeroCompra,
                    stockRestante = producto.cantidadDisponible  // Informar cuanto stock queda despues de la compra
                });
            }
            catch (Exception ex)
            {
                // Manejar cualquier error inesperado durante el proceso
                return StatusCode(500, new { mensaje = "Error al procesar la compra", error = ex.Message });
            }
        }
    }
}