using Domain.Contracts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceGeneric<T> where T : class, IEntity
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        long CountRecords();
        Task<long> CountRecordsAsync();
        void Delete(Guid id);
        Task DeleteAsync(Guid id);
        T Get(Guid id);
        Task<T> GetAsync(Guid id);
        T GetIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetIncludingAsync(Guid id, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        T Update(Guid id, T entity);
        Task<T> UpdateAsync(Guid id, T entity);
    }
}
