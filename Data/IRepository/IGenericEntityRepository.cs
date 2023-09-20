using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace AsignacionBienesINEI.Data.IRepository
{
    public interface IGenericEntityRepository<T>
    {
        Task<EntityEntry> InsertEntity<TResult>(TResult entity) where TResult : class;

        EntityState UpdateEntity<TResult>(TResult entity) where TResult : class;

        EntityEntry DeleteEntity<TResult>(TResult entity) where TResult : class;

        Task<IEnumerable<TResult>> GetAllEntity<TResult>() where TResult : class;
            
        Task<IEnumerable<TResult>> GetAllEntitiesByConditions<TResult>(Expression<Func<TResult, bool>> predicate) where TResult : class;

        Task<TResult> GetEntity<TResult>(int id) where TResult : class;        

        Task<bool> EntityExistsByAllProperties<TResult>(TResult entity) where TResult : class;

        Task<bool> EntityExistsByCustomConditions<TResult>(Expression<Func<TResult, bool>> predicate) where TResult : class;        
    }
}