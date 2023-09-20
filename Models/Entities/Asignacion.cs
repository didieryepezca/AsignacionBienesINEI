using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsignacionBienesINEI.Models.Entities
{
    public class Asignacion
    {
        [Key]
        public int Id { get; set; }        
                
        public string IdUsuario { get; set; } = string.Empty;

        [ForeignKey("IdUsuario")]
        public virtual ApplicationUser? Responsable { get; set; } = null;
        
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public virtual AsignacionEstado? Estado { get; set; } = null;

        public string? EquipoFueraINEI { get; set; } = string.Empty;

        public string? EquipoLibre { get; set; } = string.Empty;

        public string? Observacion { get; set; } = string.Empty;

        [DisplayName("Fecha de Asignación")]
        public DateTime? FechaAsignacion { get; set; }
    }
}