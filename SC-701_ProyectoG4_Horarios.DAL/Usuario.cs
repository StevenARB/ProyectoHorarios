using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    // Si es necesario una autenticacion o autorizacion para el proyecto, se utilizaria la libreria de Identity.
    public class Usuario
    {
        [Key]
        [Display(Name = "Código Usuario")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [MaxLength(100)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        // Propiedades de Navegacion

        public Profesor? Profesor { get; set; }

        public List<Reservacion>? ReservacionesCreacion { get; set; }

        public List<Reservacion>? ReservacionesModificacion { get; set; }
    }
}
