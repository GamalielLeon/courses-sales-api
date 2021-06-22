using AutoMapper;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : GenericController<User, UserRequest, UserResponse>
    {
        private readonly IServiceUser _serviceUser;
        public UsersController(IServiceUser service, IMapper mapper) : base(service, mapper)
        {
            _serviceUser = (IServiceUser)_service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            return Ok(await _serviceUser.LoginAsync(loginRequest));
        }

        [AllowAnonymous]
        [HttpPost]
        public override async Task<ActionResult<UserResponse>> PostAsync([FromBody] UserRequest entityRequest)
        {
            User user = await _serviceUser.AddUserAsync(_mapper.Map<User>(entityRequest), entityRequest.Password);
            UserCreatedResponse userResponse = _mapper.Map<UserCreatedResponse>(user);
            userResponse.Token = _serviceUser.CreateToken(user);
            return Created("", userResponse);
        }
    }
}
