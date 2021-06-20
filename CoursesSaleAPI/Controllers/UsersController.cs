using AutoMapper;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : GenericController<User, UserRequest, UserResponse>
    {
        public UsersController(IServiceUser service, IMapper mapper) : base(service, mapper)
        {
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            return Ok(_mapper.Map<LoginResponse>(await ((IServiceUser)_service).LoginAsync(loginRequest)));
        }
    }
}
