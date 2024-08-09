using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SC_701_ProyectoG4_Horarios.DAL
{

    // Si es necesario una autenticacion o autorizacion para el proyecto, se utilizaria la libreria de Identity.
    [Table("AspNetUsers")]
    public class Usuario : IdentityUser
    {

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [MaxLength(100)]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        // Propiedades de Navegacion

        public Profesor? Profesor { get; set; }

        public List<Reservacion>? ReservacionesCreacion { get; set; }

        public List<Reservacion>? ReservacionesModificacion { get; set; }
    }
}
