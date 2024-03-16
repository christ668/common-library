using common_library.Base.Context;
using common_library.Base.Model;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace common_library.Base.Repository
{
    public abstract class BaseRepository<T, TContext> : IbaseRepository<T>, IDisposable where T : class
    {
        protected readonly GenericContext<TContext> context;

        protected DbSet<T> _dBSet;

        public BaseRepository(GenericContext<TContext> dbContext)
        {
            context = dbContext ?? throw new ArgumentNullException("dbContext");
            context.SetTrackingBehavior();
            try
            {
                _dBSet = dbContext.Set<T>();
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateAsync(T entity)
        {
            await _dBSet.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dBSet.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync()
        {
            return await _dBSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dBSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dBSet.Update(entity);
            _dBSet.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public Task<List<T>> GetList<T>(Expression<Func<T, bool>> whereString) where T : class
        {
            return (_dBSet as IQueryable<T>).Where(whereString).ToListAsync();
        }

        public IEnumerable<T> GetListData(Expression<Func<T, bool>> whereString)
        {
            return _dBSet.Where(whereString).AsEnumerable();
        }

        public Task<T> GetByCriteria<T>(Expression<Func<T, bool>> whereString) where T : class
        {
            return (_dBSet as IQueryable<T>).Where(whereString).FirstOrDefaultAsync();
        }

        public async Task<(bool Added, string Message)> CreateAsyncWithValidate<T>(T entity) where T : BaseEntity
        {
            _ = 1;
            try
            {
                context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
                string msgSuccess = "Successs Insert";
                await context.SaveChangesAsync();
                return (true, msgSuccess);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return (false, "Trouble happened! \n" + ex.Message + "\n" + ex.InnerException.Message);
                }

                return (false, "Trouble happened! \n" + ex.Message);
            }
        }

        public async Task BulkInsertManyAsync<T>(List<T> entity) where T : class
        {
            await context.BulkInsertAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdatedManyAsync<T>(List<T> entities) where T : BaseEntity
        {
            foreach (T entity in entities)
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        public async Task BulkDeleteManyAsync<T>(List<T> entity) where T : class
        {
            await context.BulkDeleteAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity
        {
            return await (_dBSet as IQueryable<T>).Where(whereString).ToListAsync();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
