using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Constants;
using Domain.Contracts.Entity;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.DTOs.Pagination;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class PaginationService<TPaged> : ServiceGeneric<TPaged>, IPaginationService<TPaged> where TPaged : class, IEntity
    {
        protected readonly IGenericRepository<TPaged> _pagedRepository;
        public PaginationService(IGenericRepository<TPaged> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _pagedRepository = repository;
        }
        public virtual PaginationResponse<TPaged> GetAllPaged(PaginationRequest paginationRequest, Func<long> currentRecords)
        {
            if (paginationRequest.Page * paginationRequest.PageSize > currentRecords())
                throw new CustomException(ConstantsErrors.EXCEEDED_RECORDS, errorDescriptions[ConstantsErrors.EXCEEDED_RECORDS]);
            if (typeof(TPaged).GetProperties().Select(p => p.Name).Contains(paginationRequest.SortBy))
                throw new CustomException(ConstantsErrors.PROPERTY_ERROR, $"{paginationRequest.SortBy} property was not found");

            PaginationResponse<TPaged> paginationResponse = new PaginationResponse<TPaged>();
            paginationResponse.Results = _pagedRepository.GetAllPaged(paginationRequest).ToList();
            paginationResponse.CurrentPage = paginationRequest.Page;
            paginationResponse.PageSize = paginationRequest.PageSize;
            paginationResponse.TotalRecords = currentRecords();
            paginationResponse.TotalPages = (long)Math.Ceiling((decimal)paginationResponse.TotalRecords / paginationResponse.PageSize);
            return paginationResponse;
        }

        public virtual async Task<PaginationResponse<TPaged>> GetAllPagedAsync(PaginationRequest paginationRequest, Func<Task<long>> currentRecords)
        {
            if ((paginationRequest.Page - 1) * paginationRequest.PageSize >= await currentRecords())
                throw new CustomException(ConstantsErrors.EXCEEDED_RECORDS, errorDescriptions[ConstantsErrors.EXCEEDED_RECORDS]);
            if (!typeof(TPaged).GetProperties().Select(p => p.Name.ToLower()).Contains(paginationRequest.SortBy.ToLower()))
                throw new CustomException(ConstantsErrors.PROPERTY_ERROR, $"{paginationRequest.SortBy} property was not found");

            PaginationResponse<TPaged> paginationResponse = new PaginationResponse<TPaged>();
            paginationResponse.Results = await _pagedRepository.GetAllPagedAsync(paginationRequest);
            paginationResponse.CurrentPage = paginationRequest.Page;
            paginationResponse.PageSize = paginationRequest.PageSize;
            paginationResponse.TotalRecords = await currentRecords();
            paginationResponse.TotalPages = (long)Math.Ceiling((decimal)paginationResponse.TotalRecords / paginationResponse.PageSize);
            return paginationResponse;
        }
    }
}
