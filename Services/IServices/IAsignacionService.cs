using AsignacionBienesINEI.Models.Entities;
using AsignacionBienesINEI.Models.ViewModels;

namespace AsignacionBienesINEI.Services.IServices
{
    public interface IAsignacionService
    {
        Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesByDateAndStatus(int status, DateTime date);

        Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesPorUsuario(string userId);

        Task<IEnumerable<Bien>> SearchBienPorControlPatrimonial(string codigoPatrimonial, string tipo);

        Task<(int, int)> CreateAsignacionWithDetails(NuevaAsignacionViewModel nuevaAsignacionViewModel);

        Task<int> UpdateEquipmentStatus(NuevaAsignacionViewModel nuevaAsignacionViewModel);

        Task<int> UploadSignedAsignationPDF(IFormFile file, int idAsignacion);

        Task<int> UpdateAsignationAndReleaseEquipments(int idAsignacion, int idBien);

        Task<IEnumerable<AsignacionDetalle>> GetAsignacionPorId(int idAsignacion);

        Task<AsignacionPDF> FindAsignacionFirmadaPDF(int idAsignacion);
    }
}