using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    public class Clase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        // Propiedades de Navegacion

        public List<Reservacion>? Reservaciones { get; set; }
    }
}
