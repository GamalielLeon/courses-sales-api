using Domain.Contracts.Entity;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceGeneric<T> : IServiceGeneric<T> where T : class, IEntity
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        public ServiceGeneric(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public virtual T Add(T entity)
        {
            T entityCreated = _repository.Add(entity);
            _unitOfWork.Save();
            return entityCreated;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            T entityCreated = await _repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
            return entityCreated;
        }

        public virtual void Delete(Guid id)
        {
            _repository.Delete(_repository.Get(id));
            _unitOfWork.Save();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            _repository.Delete(await _repository.GetAsync(id));
            await _unitOfWork.SaveAsync();
        }

        public virtual T Get(Guid id)
        {
            return _repository.Get(id);
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual T Update(Guid id, T entity)
        {
            T entityOutOfDate = _repository.Get(id);
            entity.Id = entityOutOfDate.Id;
            entity.CreatedAt = entityOutOfDate.CreatedAt;
            entity.UpdatedAt = entityOutOfDate.UpdatedAt;
            entity.CreatedBy = entityOutOfDate.CreatedBy;
            entity.UpdatedBy = entityOutOfDate.UpdatedBy;

            T entityUpdated = _repository.Update(entity);
            _unitOfWork.Save();
            return entityUpdated;
        }

        public virtual async Task<T> UpdateAsync(Guid id, T entity)
        {
            T entityOutOfDate = await _repository.GetAsync(id);
            entity.Id = entityOutOfDate.Id;
            entity.CreatedAt = entityOutOfDate.CreatedAt;
            entity.UpdatedAt = entityOutOfDate.UpdatedAt;
            entity.CreatedBy = entityOutOfDate.CreatedBy;
            entity.UpdatedBy = entityOutOfDate.UpdatedBy;

            T entityUpdated = await _repository.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();
            return entityUpdated;
        }
    }
}
