using AsignacionBienesINEI.Models.Entities;

namespace AsignacionBienesINEI.Data.IRepository
{
    public interface IAsignacionDetalleRepository
    {
        Task InsertAsignacionDetalles(List<AsignacionDetalle> asignacionDetalles);
    }
}
