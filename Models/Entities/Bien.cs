using System.ComponentModel.DataAnnotations;

namespace AsignacionBienesINEI.Models.Entities
{
    public class Bien
    {
        [Key]
        [Required(ErrorMessage = "Debe tener un ID")]
        public int Id { get; set; }

        [Display(Name = "Numero")]        
        public string? Numero { get; set; } = string.Empty;        

        [Display(Name = "Plaqueta")]
        [Required(ErrorMessage = "Debe tener una Plaqueta")]
        public string Plaqueta { get; set; } = string.Empty;

        [Display(Name = "Sbn")]
        public string? Sbn { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe tener un Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Marca")]        
        public string? Marca { get; set; } = string.Empty;

        [Display(Name = "Modelo")]        
        public string? Modelo { get; set; } = string.Empty;

        [Display(Name = "Serie")]        
        public string? Serie { get; set; } = string.Empty;

        [Display(Name = "Tipo")]        
        public string? Tipo { get; set; } = string.Empty;

        [Display(Name = "Dimensiones")]        
        public string? Dimensiones { get; set; } = string.Empty;

        [Display(Name = "Color")]        
        public string? Color { get; set; } = string.Empty;

        [Display(Name = "Estado")]        
        public string? Estado { get; set; } = string.Empty;

        [Display(Name = "Operativo")]        
        public string? Operativo { get; set; } = string.Empty;

        [Display(Name = "Observaciones")]        
        public string? Observaciones { get; set; } = string.Empty;

        [Display(Name = "Proyecto")]        
        public string? Proy { get; set; } = string.Empty;

        [Display(Name = "Esta asignado?")]
        public bool Asignado { get; set; }
    }
}