using AsignacionBienesINEI.Data.IRepository;
using AsignacionBienesINEI.Models.Entities;

namespace AsignacionBienesINEI.Data.Repository
{
    public class AsignacionDetalleRepository : IAsignacionDetalleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AsignacionDetalleRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task InsertAsignacionDetalles(List<AsignacionDetalle> asignacionDetalles) 
        {
            await _applicationDbContext.AsignacionDetalle.AddRangeAsync(asignacionDetalles);
        }
    }
}