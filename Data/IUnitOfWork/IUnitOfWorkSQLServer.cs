using AsignacionBienesINEI.Data.IRepository;

namespace AsignacionBienesINEI.Data.IUnitOfWork
{
    public interface IUnitOfWorkSQLServer : IDisposable
    {
        public IAsignacionRepository AsignacionRepository { get; }

        public IAsignacionDetalleRepository AsignacionDetalleRepository { get; }

        public IGenericEntityRepository<object> GenericRepository { get; }       

        Task<int> SaveChanges();

        //Task RollBack();
    }
}