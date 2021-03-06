using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Pagination;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoursesSaleAPI.Controllers
{
    [Authorize(Roles = GlobalConstants.ROLES_ALLOWED_FOR_INSTRUCTORS_CONTROLLER)]
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class InstructorsController : PaginationController<Instructor, InstructorsPaged,InstructorRequest, InstructorResponse>
    {
        public InstructorsController(IPaginationService<InstructorsPaged> paginationService, IServiceGeneric<Instructor> service, IMapper mapper) : base(paginationService, service, mapper)
        {
        }
    }
}
