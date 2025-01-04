using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.DL.Repositories.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<T?> GetByNameAsync(string name, Func<IQueryable<T>, IQueryable<T>> include = null); //

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
