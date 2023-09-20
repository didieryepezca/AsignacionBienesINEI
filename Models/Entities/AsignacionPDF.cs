using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsignacionBienesINEI.Models.Entities
{
    public class AsignacionPDF
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("IdAsignacion")]
        public virtual Asignacion? Asignacion { get; set; } = null;

        public int IdAsignacion { get; set; }

        public byte[]? PdfData { get; set; }

        [DisplayName("Fecha de Carga")]
        public DateTime? FechaCarga { get; set; }
    }
}