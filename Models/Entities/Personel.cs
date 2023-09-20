using System.ComponentModel.DataAnnotations;

namespace AsignacionBienesINEI.Models.Entities
{
    public class Personel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Odei")]
        public string Odei { get; set; } = string.Empty;

        [Display(Name = "Apellidos y Nombres")]
        public string? ApellidosNombres { get; set; } = string.Empty;

        [Display(Name = "DNI")]
        public string? Dni { get; set; } = string.Empty;

        [Display(Name = "Condicion Laboral")]
        public string? CondicionLaboral { get; set; } = string.Empty;

        [Display(Name = "Fecha de Nacimiento")]
        public string? FechaNacimiento { get; set; } = string.Empty;

        [Display(Name = "Profesion")]
        public string? Profesion { get; set; } = string.Empty;

        [Display(Name = "Grado Academico")]
        public string? GradoAcademico { get; set; } = string.Empty;

        [Display(Name = "Celular")]
        public string? Celular { get; set; } = string.Empty;
    }
}