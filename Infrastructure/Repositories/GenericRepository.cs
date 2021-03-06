using Domain.Contracts.Entity;
using Domain.Contracts.Repository;
using Domain.DTOs.Pagination;
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

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual long CountRecords()
        {
            return _dbSet.LongCount();
        }

        public virtual async Task<long> CountRecordsAsync()
        {
            return await _dbSet.LongCountAsync();
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public virtual T FindOneIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindByIncluding(predicate, includeProperties).FirstOrDefault();
        }

        public virtual async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T> FindOneIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindByIncluding(predicate, includeProperties).FirstOrDefaultAsync();
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
            return includeProperties.Aggregate(FindBy(predicate), static(current, includeProperty) => current.Include(includeProperty));
        }

        public virtual async Task<ICollection<T>> FindByIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate(FindBy(predicate), static(current, includeProperty) => current.Include(includeProperty)).ToListAsync();
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

        public virtual IQueryable<T> GetAllPaged(PaginationRequest paginationRequest)
        {
            string parameters = $"@Page = {paginationRequest.Page}, @PageSize = {paginationRequest.PageSize}";
            parameters += $", @SortBy = {paginationRequest.SortBy}, @IsSortDescendent = {paginationRequest.IsSortDescendent}";
            return _dbSet.FromSqlRaw($"usp_{typeof(T).Name} {parameters}");
        }

        public virtual async Task<ICollection<T>> GetAllPagedAsync(PaginationRequest paginationRequest)
        {
            string parameters = $"@Page = {paginationRequest.Page}, @PageSize = {paginationRequest.PageSize}";
            parameters += $", @SortBy = {paginationRequest.SortBy}, @IsSortDescendent = {paginationRequest.IsSortDescendent}";
            return await _dbSet.FromSqlRaw($"usp_{typeof(T).Name} {parameters}").ToListAsync();
        }

        public virtual T GetIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            return FindOneIncluding(e => e.Id == id, includeProperties);
        }

        public virtual async Task<T> GetIncludingAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindOneIncludingAsync(e => e.Id == id, includeProperties);
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(GetAll(), static(current, includeProperty) => current.Include(includeProperty));
        }

        public virtual async Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await includeProperties.Aggregate(GetAll(), static(current, includeProperty) => current.Include(includeProperty)).ToListAsync();
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
