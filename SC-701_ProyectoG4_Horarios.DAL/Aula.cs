using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    public class Aula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }

        [Required]
        public int Capacidad { get; set; }

        // Propiedades de Navegacion

        public List<Reservacion>? Reservaciones { get; set; }
    }
}
