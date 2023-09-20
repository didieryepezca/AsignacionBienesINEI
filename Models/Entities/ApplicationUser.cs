using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace AsignacionBienesINEI.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {       
        [Display(Name = "ODEI")]
        public string Odei { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Display(Name = "DNI")]
        [Required]
        public string DNI { get; set; } = string.Empty;

        [Display(Name = "Condición Laboral")]
        public string CondicionLaboral { get; set; } = string.Empty;

        [Display(Name = "Fecha de Nacimiento")]        
        public string FechaNacimiento { get; set; } = string.Empty;

        [Display(Name = "Profesión/Estudio")]
        public string ProfesionEstudio { get; set; } = string.Empty;

        [Display(Name = "Grado Académico")]
        public string GradoAcademico { get; set; } = string.Empty;

        [Display(Name = "Teléfono/Celular")]
        public string TelefonoCelular { get; set; } = string.Empty;

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        public string UsuarioNombreCompleto
        {
            get { return Nombres + " " + ApellidoPaterno + " " + ApellidoMaterno; }
        }
    }
}