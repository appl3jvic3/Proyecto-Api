using Api_de_Prueba.Modelos;
using Microsoft.EntityFrameworkCore;

namespace Api_de_Prueba.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarritoCompra> RegistrosDeCompra { get; set; }

        public DbSet<Producto> ManejoDeProductos { get; set; }

        public DbSet<Usuario> RegistrarUsuarios { get; set; }
    }
}
