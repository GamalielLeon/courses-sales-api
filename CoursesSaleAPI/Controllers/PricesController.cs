using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Pagination;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoursesSaleAPI.Controllers
{
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class PricesController : GenericController<Price, PricesPaged, PriceRequest, PriceResponse>
    {
        public PricesController(IServiceGeneric<Price, PricesPaged> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
