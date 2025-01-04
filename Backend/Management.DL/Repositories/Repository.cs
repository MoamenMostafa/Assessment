using Management.DL.Context;
using Management.DL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.DL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            var query = _dbSet.AsQueryable();
            if (include != null)
            {
                query = include(query);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            var query = _dbSet.AsQueryable();
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        public async Task<T?> GetByNameAsync(string name, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            var query = _dbSet.AsQueryable();

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
