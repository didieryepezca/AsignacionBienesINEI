using AsignacionBienesINEI.Models.Entities;

namespace AsignacionBienesINEI.Data.IRepository
{
    public interface IAsignacionRepository
    {
        Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesByEstadoAndFecha(int idEstado, DateTime fechaAsignacion);

        Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesPorUsuario(string userId);

        Task<IEnumerable<Bien>> SearchBienPorControlPatrimonial(string codigoPatrimonial, string tipo);

        Task<Asignacion> InsertAsignacion(Asignacion asignacion);

        Task<IEnumerable<AsignacionDetalle>> GetAsignacionPorId(int idAsignacion);

        Task<AsignacionPDF> FindAsignacionFirmadaPDF(int idAsignacion);
    }
}