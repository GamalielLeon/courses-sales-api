using AutoMapper;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoursesSaleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : GenericController<Instructor, InstructorRequest, InstructorResponse>
    {
        public InstructorsController(IServiceGeneric<Instructor> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}
