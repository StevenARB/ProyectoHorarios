using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SC_701_ProyectoG4_Horarios.DAL
{
    public class Profesor
    {
        [Key]
        [Display(Name = "Código Profesor")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Departamento { get; set; } // Ciencias, Matemáticas...

        [Required]
        [MaxLength(100)]
        public string Titulo { get; set;} // Profesor, Doctor...

        public string? UsuarioId { get; set; }

        // Propiedades de Navegacion

        [BindNever]
        public Usuario? Usuario { get; set; }

        [BindNever]
        public List<Reservacion>? Reservaciones { get; set; }
    }
}
