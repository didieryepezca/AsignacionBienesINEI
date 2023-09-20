using System.ComponentModel;

namespace AsignacionBienesINEI.Models.ViewModels
{
    public class AsignacionesPorUsuarioViewModel
    {
        public string Equipo { get; set; } = string.Empty;

        public string Marca { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public string CP { get; set; } = string.Empty;

        [DisplayName("Dirección Técnica")]
        public string DireccionTecnica { get; set; } = string.Empty;

        public string Fecha { get; set; } = string.Empty;
    }
}