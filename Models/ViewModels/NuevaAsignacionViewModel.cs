using System.ComponentModel.DataAnnotations;

namespace AsignacionBienesINEI.Models.ViewModels
{
    public class NuevaAsignacionViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        [DataType(DataType.Text)]
        public string idUsuario { get; set; } = string.Empty;

        public int idResultingFromDb { get; set; } 

        public string? usuarioNombres { get; set; } = string.Empty;

        public string? usuarioUserName { get; set; } = string.Empty;

        public string? usuarioDNI { get; set; } = string.Empty;

        public string? usuarioOdei { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica si estará fuera del INEI")]
        public string equipoFueraINEI { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indica si está libre")]
        public string equipoLibre { get; set; } = string.Empty;

        public string? observacion { get; set; }

        public List<ResponsablesViewModel>? responsables { get; set; }

        //los newList<etc...> solamente es para evitar la advertencia de no contar con NULLS.
        public List<AsignacionesAgregadasViewModel> asignacionesAgregadas { get; set; } = new List<AsignacionesAgregadasViewModel>();

        public List<AsignacionesPorUsuarioViewModel>? asignacionesPorUsuario { get; set; } = new List<AsignacionesPorUsuarioViewModel>();        
    }
}