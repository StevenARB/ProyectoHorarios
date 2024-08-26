using System.ComponentModel.DataAnnotations;

namespace SC_701_ProyectoG4_Horarios.Models
{
    public class AdminEditarUsuarioViewModel
    {
        [Required]
        public string Id { get; set; }

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
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "La {0} debe ser como mínimo {2} y máximo {1} carácteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Antigua")]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessage = "La {0} debe ser como mínimo {2} y máximo {1} carácteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide con la contraseña de confirmación.")]
        public string ConfirmPassword { get; set; }

        public string IdRol { get; set; }
    }
}
