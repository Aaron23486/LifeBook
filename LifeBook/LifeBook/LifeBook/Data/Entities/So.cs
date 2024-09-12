using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LifeBook.Data.Entities
{
    public class So
    {
        public int Id { get; set; } // Identificador único

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; } // Nombre del sistema operativo

        public string? Description { get; set; } // Descripción opcional del sistema operativo

        // Relación con ataques y herramientas
        public List<Attack>? Attacks { get; set; }
        public List<Tool>? Tools { get; set; }
    }
}
