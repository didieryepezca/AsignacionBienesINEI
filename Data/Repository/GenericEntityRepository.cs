using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AsignacionBienesINEI.Data.IRepository;

namespace AsignacionBienesINEI.Data.Repository
{
    public class GenericEntityRepository : IGenericEntityRepository<object>
    {
        private readonly ApplicationDbContext _appDbContext;

        public GenericEntityRepository(ApplicationDbContext applicationDbContext)
        {
            _appDbContext = applicationDbContext;
        }

        public async Task<EntityEntry> InsertEntity<TResult>(TResult entity) where TResult : class
        {
            var insert = await _appDbContext.Set<TResult>().AddAsync(entity);
            return insert;
        }

        public EntityState UpdateEntity<T>(T entity) where T : class
        {
            var entry = _appDbContext.Entry(entity).State = EntityState.Modified;
            return entry;
        }

        public EntityEntry DeleteEntity<T>(T entity) where T : class
        {
            var remove = _appDbContext.Set<T>().Remove(entity);
            return remove;
        }

        public async Task<IEnumerable<T>> GetAllEntity<T>() where T : class
        {
            var objects = await _appDbContext.Set<T>().ToListAsync();
            return objects;
        }

        public async Task<IEnumerable<T>> GetAllEntitiesByConditions<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            IEnumerable<T> result;
            result = await _appDbContext.Set<T>().AsQueryable().Where(predicate).ToListAsync();
            return result;
        }

        public async Task<T> GetEntity<T>(int id) where T : class
        {
            var entity = await _appDbContext.Set<T>().FindAsync(id);
            return entity!;
        }       

        public async Task<bool> EntityExistsByAllProperties<T>(T entity) where T : class
        {
            return await _appDbContext.Set<T>().AnyAsync(e => e == entity);
        }

        public async Task<bool> EntityExistsByCustomConditions<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _appDbContext.Set<T>().AsQueryable().AnyAsync(predicate);
        }        
    }
}