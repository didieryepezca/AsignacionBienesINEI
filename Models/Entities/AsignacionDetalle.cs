using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsignacionBienesINEI.Models.Entities
{
    public class AsignacionDetalle
    {
        [Key]
        public int Id { get; set; }
        
        public int IdAsignacion { get; set; }

        [ForeignKey("IdAsignacion")]
        public virtual Asignacion? Asignacion { get; set; } = null;
        
        public int IdBien { get; set; }

        [ForeignKey("IdBien")]
        public virtual Bien? Bien { get; set; } = null;

        public DateTime FechaIngreso { get; set; }
    }
}