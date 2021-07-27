using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class UserRolesController : GenericController<UserRole, UserRoleRequest, UserRoleResponse>
    {
        private readonly IServiceUserRole _serviceUserRole;
        public UserRolesController(IServiceUserRole service, IMapper mapper) : base(service, mapper)
        {
            _serviceUserRole = (IServiceUserRole)_service;
        }

        [HttpPost("AddRolesToUser")]
        public async Task<ActionResult<UserRoleResponse>> AddRolesToUserAsync([FromBody] UserRoleRequest userRoleRequest)
        {
            UserRoleResponse userWithRoles = new UserRoleResponse();
            userWithRoles.User = _mapper.Map<UserResponse>(await _serviceUserRole.AddRolesToUserAsync(userRoleRequest));
            userWithRoles.Roles = _mapper.Map<IEnumerable<RoleResponse>>(await _serviceUserRole.GetUserRolesAsync(ur => ur.UserId == userWithRoles.User.Id));
            return Ok(userWithRoles);
        }

        [HttpDelete("RemoveRolesFromUser")]
        public async Task<ActionResult<UserRoleResponse>> RemoveRolesFromUserAsync([FromBody] UserRoleRequest userRoleRequest)
        {
            UserRoleResponse userWithRoles = new UserRoleResponse();
            userWithRoles.User = _mapper.Map<UserResponse>(await _serviceUserRole.RemoveRolesFromUserAsync(userRoleRequest));
            userWithRoles.Roles = _mapper.Map<IEnumerable<RoleResponse>>(await _serviceUserRole.GetUserRolesAsync(ur => ur.UserId == userWithRoles.User.Id));
            return Ok(userWithRoles);
        }
    }
}
