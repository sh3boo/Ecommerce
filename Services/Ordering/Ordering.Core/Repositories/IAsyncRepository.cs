using Ordering.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Repositories
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync (int id);
        //Task<T> GetByNameAsync (string name);
        Task<T> AddAsync (T entity);
        Task<T> UpdateAsync (T entity);
        Task DeleteAsync (T entity);
    }
}
