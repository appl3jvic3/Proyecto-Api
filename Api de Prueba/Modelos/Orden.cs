using System.ComponentModel.DataAnnotations;

namespace Api_de_Prueba.Modelos
    {
        public class Orden
        {
            [Key]
            public int ordenId { get; set; }

            public int usuarioId { get; set; }

            public DateTime fecha { get; set; } = DateTime.UtcNow;

            // total calculado a partir de los items
            public double total { get; set; }

            // Navigation property
            public List<OrdenItem> items { get; set; } = new();
        }
    
}
