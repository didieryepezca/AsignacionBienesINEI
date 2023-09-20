using System.ComponentModel.DataAnnotations;

namespace AsignacionBienesINEI.Models.Entities
{
    public class AsignacionEstado
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
    }
}