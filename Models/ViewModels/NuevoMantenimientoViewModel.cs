using System.ComponentModel.DataAnnotations;

namespace AsignacionBienesINEI.Models.ViewModels
{
    public class NuevoMantenimientoViewModel
    {
        public int idResultingFromDb { get; set; }

        public DateTime fechaRegistro { get; set; }

        [Required(ErrorMessage = "Indica la fecha en la que se hará el mantenimiento")]
        public DateTime? fechaMantenimiento { get; set; }

        public string? Odei { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica el nombre del responsable")]
        public string? responsable { get; set; } = string.Empty;

        public string? observacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica si el bien cuenta con garantía")]
        public string? garantia { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica el estado del bien")]
        public string estado { get; set; } = string.Empty;  

        public List<string> estados { get; set; } = new List<string> { "BUENO", "REGULAR", "MALO", "OBSOLETO" };

        [Required(ErrorMessage = "Indica la operatividad del bien")]
        public bool? operatividad { get; set; }

        public List<KeyValuePair<string, bool>> operatividades { get; set; } = new List<KeyValuePair<string, bool>>
        {
            new KeyValuePair<string, bool>("OPERATIVO", true),
            new KeyValuePair<string, bool>("INOPERATIVO", false)
        };

        [Required(ErrorMessage = "Indica si el bien está en uso")]
        public bool? equipoUso { get; set; }

        public List<KeyValuePair<string, bool>> usos { get; set; } = new List<KeyValuePair<string, bool>>
        {
            new KeyValuePair<string, bool>("SI", true),
            new KeyValuePair<string, bool>("NO", false)
        };

        [Required(ErrorMessage = "Indica si el bien está dado de baja")]
        public bool? equipoDadoBaja { get; set; }

        public List<KeyValuePair<string, bool>> bajas { get; set; } = new List<KeyValuePair<string, bool>>
        {
            new KeyValuePair<string, bool>("SI", true),
            new KeyValuePair<string, bool>("NO", false)
        };
        
        [Required(ErrorMessage = "Selecciona un equipo de la lista.")]
        public List<AsignacionesAgregadasViewModel> equipoAgregado { get; set; } = new List<AsignacionesAgregadasViewModel>();
    }
}