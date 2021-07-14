using Domain.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceGeneric<T, TPaginated> where T : class where TPaginated : class
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
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
        PaginationResponse<TPaginated> GetAllPaged(PaginationRequest paginationRequest);
        Task<PaginationResponse<TPaginated>> GetAllPagedAsync(PaginationRequest paginationRequest);
        T Update(Guid id, T entity);
        Task<T> UpdateAsync(Guid id, T entity);
    }
}
