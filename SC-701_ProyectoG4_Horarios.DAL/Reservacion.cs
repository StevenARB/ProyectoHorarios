using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    public class Reservacion
    {
        [Key]
        [Display(Name = "Código Reservación")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Código Aula")]
        public int AulaId { get; set; }

        [Required]
        [Display(Name = "Código Profesor")]
        public int ProfesorId { get; set; }

        [Display(Name = "Código Clase")]
        public int? ClaseId { get; set; }

        [Required]
        public DateOnly Fecha { get; set; }

        [Required]
        [Display(Name = "Hora Inicio")]
        public TimeOnly HoraInicio { get; set; }

        [Required]
        [Display(Name = "Hora Fin")]
        public TimeOnly HoraFin { get; set; }

        [Required]
        [Display(Name = "Código Usuario Creación")]
        public string UsuarioCreacionId { get; set; }

        [Required]
        [Display(Name = "Fecha Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Código Usuario Modificación")]
        public string? UsuarioModificacionId { get; set; }

        [Display(Name = "Fecha Modificación")]
        public DateTime? FechaModificacion { get; set; }

        // Propiedades de Navegacion

        public Aula? Aula { get; set; }

        public Profesor? Profesor { get; set; }

        public Clase? Clase { get; set; }

        public Usuario? UsuarioCreacion { get; set; }

        public Usuario? UsuarioModificacion { get; set; }
    }
}
