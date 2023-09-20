using AsignacionBienesINEI.Data.IRepository;
using AsignacionBienesINEI.Data.IUnitOfWork;

namespace AsignacionBienesINEI.Data.UnitOfWork
{
    public class UnitOfWorkSQLServer : IUnitOfWorkSQLServer
    {
        private readonly ApplicationDbContext _applicationDbContext;        

        public IAsignacionRepository AsignacionRepository { get; }

        public IAsignacionDetalleRepository AsignacionDetalleRepository { get; }

        public IGenericEntityRepository<object> GenericRepository { get; }
              

        public UnitOfWorkSQLServer(ApplicationDbContext applicationDbContext, IAsignacionRepository asignacionRepository,
            IAsignacionDetalleRepository asignacionDetalleRepository, IGenericEntityRepository<object> genericEntityRepository) 
        {
            _applicationDbContext = applicationDbContext;           
            AsignacionRepository = asignacionRepository;
            AsignacionDetalleRepository = asignacionDetalleRepository;
            GenericRepository = genericEntityRepository;           
        }       

        public void Dispose() 
        {
            if(_applicationDbContext != null)
                _applicationDbContext.Dispose();          
        }

        public async Task<int> SaveChanges()
            => await _applicationDbContext.SaveChangesAsync();        

        //public async Task CommitChanges()
        //    => await _transaction.CommitAsync();        

    }
}