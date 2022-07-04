using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Model.Entity;

namespace Tadrebat.Persistance.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> FindAsync(params object[] keyvalues);
        Task<T> GetByIDAsync(Guid ID);
        Task<T> GetByIDAsync(long ID);
        Task<IEnumerable<T>> ListAllAsync();

        IEnumerable<T> ListAll();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetQueryableFirstorDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetQueryableFirstorDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> order, bool IsAscending);
        Task<IEnumerable<T>> GetQueryableTolistAsync(Expression<Func<T, bool>> predicate);

    }
}
