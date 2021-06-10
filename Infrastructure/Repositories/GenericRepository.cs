using Domain.Contracts.Entity;
using Domain.Contracts.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected readonly OnlineCoursesContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(OnlineCoursesContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        //public void DeleteRange(IEnumerable<T> entities)
        //{
        //    _table.RemoveRange(entities);
        //}

        public virtual T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual IQueryable<T> FindByIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(FindBy(predicate), (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual async Task<ICollection<T>> FindByIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate(FindBy(predicate), (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
        }

        public virtual T Get(Guid id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual T GetIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindByIncluding(e => e.Id == id, includeProperties).FirstOrDefault();
        }

        public virtual async Task<T> GetIncludingAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindByIncluding(e => e.Id == id, includeProperties).FirstOrDefaultAsync();
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(GetAll(), (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual async Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate(GetAll(), (current, includeProperty) => current.Include(includeProperty)).ToListAsync();
        }

        public virtual T Update(T entity)
        {
            if (entity == null) return null;
            T entityExists = _dbSet.Find(entity.Id);
            if (entityExists != null) _context.Entry(entityExists).CurrentValues.SetValues(entity);
            return entityExists;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null) return null;
            T entityExists = await _dbSet.FindAsync(entity.Id);
            if (entityExists != null) _context.Entry(entityExists).CurrentValues.SetValues(entity);
            return entityExists;
        }
    }
}
