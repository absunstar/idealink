using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tadrebat.Model.Entity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Tadrebat.Persistance.Interfaces;


namespace Tadrebat.Persistance.Data
{
    public class TadrebatRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly TadrebatDbContext _dbContext;

        public TadrebatRepository(TadrebatDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<T> GetByIDAsync(Guid ID)
        {
            return await _dbContext.Set<T>().FindAsync(ID);
        }
        public async Task<T> GetByIDAsync(long ID)
        {
            return await _dbContext.Set<T>().FindAsync(ID);
        }

        public async Task<T> FindAsync(params object[] keyvalues)
        {
            return await _dbContext.Set<T>().FindAsync(keyvalues);
        }

        public async Task<T> GetQueryableFirstorDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsQueryable().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetQueryableFirstorDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, Object>> order,bool IsAscending)
        {
            if(IsAscending)
                return await _dbContext.Set<T>().AsQueryable().Where(predicate).OrderBy(order).FirstOrDefaultAsync();

            return await _dbContext.Set<T>().AsQueryable().Where(predicate).OrderByDescending(order).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetQueryableTolistAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsQueryable().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public IEnumerable<T> ListAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}

