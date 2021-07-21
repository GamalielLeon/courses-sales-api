using AutoMapper;
using Domain.Contracts.Entity;
using Domain.Contracts.Service;
using Domain.DTOs.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaginationController<T, TPaged, TRequest, TResponse> : GenericController<T, TRequest, TResponse> where T : class, IEntity where TPaged : class, IEntity where TRequest : class where TResponse : class
    {
        protected readonly IPaginationService<TPaged> _paginationService;
        public PaginationController(IPaginationService<TPaged> paginationService, IServiceGeneric<T> service, IMapper mapper) : base(service, mapper)
        {
            _paginationService = paginationService;
        }

        [HttpGet("GetPaged")]
        public virtual async Task<ActionResult<PaginationResponse<TPaged>>> GetAllPagedAsync([FromQuery] PaginationRequest paginationRequest)
        {
            return Ok(await _paginationService.GetAllPagedAsync(paginationRequest, _service.CountRecordsAsync));
        }
    }
}
