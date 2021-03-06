using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Constants;
using Domain.Contracts.Entity;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceGeneric<T> : IServiceGeneric<T> where T : class, IEntity
    {
        protected readonly CustomException CustomExceptionNotFound404 = new CustomException(ConstantsErrors.NOT_FOUND, ConstantsErrors.ERROR_DESCRIPTIONS[ConstantsErrors.NOT_FOUND], Code.Error404);
        protected readonly Dictionary<string, string> errorDescriptions = ConstantsErrors.ERROR_DESCRIPTIONS;
        protected readonly IGenericRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        
        public ServiceGeneric(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        //Returns a CustomException if the 'ex' passed was yielded due to a duplicate value. Otherwise, returns the original 'ex'.
        protected Exception CheckExceptionforDuplicateValue(Exception ex, string entityName)
        {
            Exception exception = ex is DbUpdateException ? ex.InnerException : ex;
            if (exception.Message.Contains("duplicate key"))
            {
                string duplicateField = exception.Message.Split($"IX_{entityName}_").LastOrDefault().Split("'.").FirstOrDefault();
                return new CustomException(ConstantsErrors.DUPLICATE_VALUE, $"This {duplicateField} already exists");
            }
            return ex;
        }

        public virtual T Add(T entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                T entityCreated = _repository.Add(entity);
                _unitOfWork.Save();
                return entityCreated;
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, typeof(T).Name);
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.CreatedAt = DateTime.Now;
                T entityCreated = await _repository.AddAsync(entity);
                await _unitOfWork.SaveAsync();
                return entityCreated;
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, typeof(T).Name);
            }
        }

        public virtual long CountRecords()
        {
            return _repository.CountRecords();
        }

        public virtual async Task<long> CountRecordsAsync()
        {
            return await _repository.CountRecordsAsync();
        }

        public virtual void Delete(Guid id)
        {
            T entityToDelete = _repository.Get(id);
            if (entityToDelete == null)
                throw CustomExceptionNotFound404;
            _repository.Delete(entityToDelete);
            _unitOfWork.Save();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            T entityToDelete = await _repository.GetAsync(id);
            if (entityToDelete == null) throw CustomExceptionNotFound404;
            _repository.Delete(entityToDelete);
            await _unitOfWork.SaveAsync();
        }

        public virtual T Get(Guid id)
        {
            T entity = _repository.Get(id);
            return entity ?? throw CustomExceptionNotFound404;
        }

        public virtual async Task<T> GetAsync(Guid id)
        {
            T entity = await _repository.GetAsync(id);
            return entity ?? throw CustomExceptionNotFound404;
        }

        public virtual T GetIncluding(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            T entity = _repository.GetIncluding(id, includeProperties);
            return entity ?? throw CustomExceptionNotFound404;
        }

        public virtual async Task<T> GetIncludingAsync(Guid id, params Expression<Func<T, object>>[] includeProperties)
        {
            T entity = await _repository.GetIncludingAsync(id, includeProperties);
            return entity ?? throw CustomExceptionNotFound404;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return _repository.GetAllIncluding(includeProperties);
        }

        public virtual async Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await _repository.GetAllIncludingAsync(includeProperties);
        }

        public virtual T Update(Guid id, T entity)
        {
            T entityOutOfDate = _repository.Get(id);
            if (entityOutOfDate == null) throw CustomExceptionNotFound404;

            entity.Id = entityOutOfDate.Id;
            entity.CreatedAt = entityOutOfDate.CreatedAt;
            entity.UpdatedAt = DateTime.Now;
            entity.CreatedBy = entityOutOfDate.CreatedBy;
            entity.UpdatedBy = entityOutOfDate.UpdatedBy;

            try
            {
                T entityUpdated = _repository.Update(entity);
                _unitOfWork.Save();
                return entityUpdated;
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, typeof(T).Name);
            }
        }

        public virtual async Task<T> UpdateAsync(Guid id, T entity)
        {
            T entityOutOfDate = await _repository.GetAsync(id);
            if (entityOutOfDate == null) throw CustomExceptionNotFound404;

            entity.Id = entityOutOfDate.Id;
            entity.CreatedAt = entityOutOfDate.CreatedAt;
            entity.UpdatedAt = DateTime.Now;
            entity.CreatedBy = entityOutOfDate.CreatedBy;
            entity.UpdatedBy = entityOutOfDate.UpdatedBy;

            try
            {
                T entityUpdated = await _repository.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                return entityUpdated;
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, typeof(T).Name);
            }
        }
    }
}
