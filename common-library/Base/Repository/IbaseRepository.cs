using common_library.Base.Model;
using System.Linq.Expressions;

namespace common_library.Base.Repository
{
    public interface IbaseRepository<T> : IDisposable where T : class
    {
        Task<IReadOnlyList<T>> GetAsync();

        Task<T> GetByIdAsync(object id);

        Task<List<T>> GetList<T>(Expression<Func<T, bool>> whereString) where T : class;

        Task<T> GetByCriteria<T>(Expression<Func<T, bool>> whereString) where T : class;

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<(bool Added, string Message)> CreateAsyncWithValidate<T>(T entity) where T : BaseEntity;

        Task BulkInsertManyAsync<T>(List<T> entity) where T : class;

        Task UpdatedManyAsync<T>(List<T> entity) where T : BaseEntity;

        Task BulkDeleteManyAsync<T>(List<T> entity) where T : class;

        Task<List<T>> ListAsyncWithWhere<T>(Expression<Func<T, bool>> whereString) where T : BaseEntity;
       
        IEnumerable<T> GetListData(Expression<Func<T, bool>> whereString);
    }
}
