using AsignacionBienesINEI.Data.IRepository;
using AsignacionBienesINEI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AsignacionBienesINEI.Data.Repository
{
    public class AsignacionRepository : IAsignacionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AsignacionRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesByEstadoAndFecha(int idEstado, DateTime fechaAsignacion)
        {
            IEnumerable<AsignacionDetalle> query = await _applicationDbContext.AsignacionDetalle
                .Where(items => items.FechaIngreso.Date == fechaAsignacion.Date) //Prevent to returning millions of rows if there are
                .Include(a => a.Asignacion)
                .Include(b => b.Bien)
                .Include(e => e.Asignacion!.Responsable)
                .ToListAsync();

            if (idEstado > 0)
            {
                query = query.Where(items => items.Asignacion!.Estado!.Id == idEstado);
            }
            return query; 
        }

        public async Task<IEnumerable<AsignacionDetalle>> GetAllAsignacionesPorUsuario(string userId)
        {
            IEnumerable<AsignacionDetalle> query = await _applicationDbContext.AsignacionDetalle
                .Where(items => items.Asignacion!.Responsable!.Id == userId && items.Bien!.Asignado == true)
                .Include (a => a.Asignacion!)
                .Include(b => b.Bien)
                .Include(e => e.Asignacion!.Responsable)
                .ToListAsync();

            query = query.OrderBy(f => f.Asignacion!.FechaAsignacion);
            return query;
        }

        public async Task<IEnumerable<AsignacionDetalle>> GetAsignacionPorId(int idAsignacion)
        {
            //IEnumerable<AsignacionDetalle> query1 = _appDbContext.Set<AsignacionDetalle>().AsQueryable();
            //query1 = await _appDbContext.AsignacionDetalle
            //    .Where(items => items.Asignacion!.Id == idAsignacion)
            //    .Include(a => a.Asignacion)
            //    .Include(b => b.Bien)
            //    .Include(e => e.Asignacion!.Responsable)
            //    .ToListAsync();

            IEnumerable<AsignacionDetalle> query = await _applicationDbContext.AsignacionDetalle
                .Where(items => items.Asignacion!.Id == idAsignacion)
                .Include(a => a.Asignacion)
                .Include(b => b.Bien)
                .Include(e => e.Asignacion!.Responsable)
                .ToListAsync();

            query = query.OrderBy(f => f.Asignacion!.FechaAsignacion);
            return query;
        }

        public async Task<IEnumerable<Bien>> SearchBienPorControlPatrimonial(string codigoPatrimonial, string tipo)
        {
            IEnumerable<Bien> query = _applicationDbContext.Set<Bien>().AsQueryable();
            query = await _applicationDbContext.Bien.ToListAsync();

            if (!string.IsNullOrEmpty(codigoPatrimonial))
            {
                query = query.Where(items => items.Plaqueta.StartsWith(codigoPatrimonial));
            }

            if (tipo == "A") 
            {
                query = query.Where(item => item.Asignado == false);            
            }            
            return query;
        }

        public async Task<Asignacion> InsertAsignacion(Asignacion asignacion)
        {
            EntityEntry<Asignacion> insertedAsignacion = await _applicationDbContext.Asignacion.AddAsync(asignacion);
            await _applicationDbContext.SaveChangesAsync();

            return insertedAsignacion.Entity;     
        }
        
        public async Task<AsignacionPDF> FindAsignacionFirmadaPDF(int idAsignacion) 
        {
            AsignacionPDF? asignacionPDF = await _applicationDbContext.AsignacionPDF
                .Where(a => a.IdAsignacion == idAsignacion).FirstOrDefaultAsync();
            return asignacionPDF!;        
        }    
    }
}