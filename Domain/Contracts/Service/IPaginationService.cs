using Domain.Contracts.Entity;
using Domain.DTOs.Pagination;
using System;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IPaginationService<TPaged> : IServiceGeneric<TPaged> where TPaged : class, IEntity
    {
        PaginationResponse<TPaged> GetAllPaged(PaginationRequest paginationRequest, Func<long> currentRecords);
        Task<PaginationResponse<TPaged>> GetAllPagedAsync(PaginationRequest paginationRequest, Func<Task<long>> currentRecords);
    }
}
