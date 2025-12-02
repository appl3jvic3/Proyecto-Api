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

        public DbSet<CarritoCompra> CarritoCompra { get; set; }

        public DbSet<Producto> Producto { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        
    }
}
