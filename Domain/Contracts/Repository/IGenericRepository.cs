using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        T FindOne(Expression<Func<T, bool>> predicate);
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<ICollection<T>> FindByIncludingAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        T Get(Guid id);
        Task<T> GetAsync(Guid id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        T GetIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetIncludingAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        T Update(T entity);
        Task<T> UpdateAsync(T entity);

        //public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        //{
        //    var query = GetAll().Where(predicate);
        //    return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        //}
    }
}
