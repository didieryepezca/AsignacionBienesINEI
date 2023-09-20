using System.ComponentModel.DataAnnotations;

namespace AsignacionBienesINEI.Models.ViewModels
{
    public class AsignacionesAgregadasViewModel
    {
        [Required(ErrorMessage = "El objeto debe tener un Id")]
        public int Id { get; set; }
        
        public string CP { get; set; } = string.Empty;
        
        public string Marca { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public string DireccionTecnica { get; set; } = string.Empty;

        public DateTime FechaIngreso { get; set; }
    }
}