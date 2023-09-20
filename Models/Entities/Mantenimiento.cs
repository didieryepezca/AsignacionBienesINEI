using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsignacionBienesINEI.Models.Entities
{
    public class Mantenimiento
    {
        [Key]
        public int Id { get; set; }

        public int IdBien { get; set; } 

        [ForeignKey("IdBien")]
        public virtual Bien? Bien { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime FechaMantenimiento { get; set; }

        public string Odei { get; set; } = default!;

        public string Responsable { get; set; } = default!;

        public string? Observacion { get; set; } 

        public string? Garantia { get; set; }

        public string? Estado { get; set; }

        public bool Operatividad { get; set; }

        public bool EquipoUso { get; set; }

        public bool EquipoDadoBaja { get; set; }
    }
}