using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceGeneric<T> where T : class
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void Delete(Guid id);
        Task DeleteAsync(Guid id);
        T Get(Guid id);
        Task<T> GetAsync(Guid id);
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();
        T Update(Guid id, T entity);
        Task<T> UpdateAsync(Guid id, T entity);
    }
}
