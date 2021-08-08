using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Authorize(Roles = GlobalConstants.ROLES_ALLOWED_FOR_USER_ROLES_CONTROLLER)]
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class UserRolesController : GenericController<UserRole, UserRoleRequest, UserRoleResponse>
    {
        private readonly IServiceUserRole _serviceUserRole;
        public UserRolesController(IServiceUserRole service, IMapper mapper) : base(service, mapper)
        {
            _serviceUserRole = (IServiceUserRole)_service;
        }

        [HttpGet("GetUsersWithRoles")]
        public async Task<ActionResult<IEnumerable<UserRoleResponse>>> GetUsersWithRolesAsync()
        {
            ICollection<UserRoleResponse> usersWithRoles = new List<UserRoleResponse>();
            foreach (User user in await _serviceUserRole.GetUsersAsync())
            {
                usersWithRoles.Add(new UserRoleResponse()
                {
                    User = _mapper.Map<UserResponse>(user),
                    Roles = await GetUserRolesMappedAsync(user.Id)
                });
            }
            return Ok(usersWithRoles);
        }

        [Authorize(Roles = GlobalConstants.USER_ROLE)]
        [HttpGet("GetUserWithRolesById/{id}")]
        public async Task<ActionResult<UserRoleResponse>> GetUserWithRolesByIdAsync(Guid id)
        {
            UserRoleResponse userWithRoles = new UserRoleResponse();
            userWithRoles.User = _mapper.Map<UserResponse>(await _serviceUserRole.GetUserByIdOrUserNameAsync(id));
            userWithRoles.Roles = await GetUserRolesMappedAsync(userWithRoles.User.Id);
            return Ok(userWithRoles);
        }

        [Authorize(Roles = GlobalConstants.USER_ROLE)]
        [HttpGet("GetUserWithRolesByUserName/{username}")]
        public async Task<ActionResult<UserRoleResponse>> GetUserWithRolesByUserNameAsync(string username)
        {
            UserRoleResponse userWithRoles = new UserRoleResponse();
            userWithRoles.User = _mapper.Map<UserResponse>(await _serviceUserRole.GetUserByIdOrUserNameAsync(username));
            userWithRoles.Roles = await GetUserRolesMappedAsync(userWithRoles.User.Id);
            return Ok(userWithRoles);
        }

        [HttpPost("AddRolesToUser")]
        public async Task<ActionResult<UserRoleResponse>> AddRolesToUserAsync([FromBody] UserRoleRequest userRoleRequest)
        {
            UserRoleResponse userWithRoles = new UserRoleResponse();
            userWithRoles.User = _mapper.Map<UserResponse>(await _serviceUserRole.AddRolesToUserAsync(userRoleRequest));
            userWithRoles.Roles = await GetUserRolesMappedAsync(userWithRoles.User.Id);
            return Ok(userWithRoles);
        }

        [HttpDelete("RemoveRolesFromUser")]
        public async Task<ActionResult<UserRoleResponse>> RemoveRolesFromUserAsync([FromBody] UserRoleRequest userRoleRequest)
        {
            UserRoleResponse userWithRoles = new UserRoleResponse();
            userWithRoles.User = _mapper.Map<UserResponse>(await _serviceUserRole.RemoveRolesFromUserAsync(userRoleRequest));
            userWithRoles.Roles = await GetUserRolesMappedAsync(userWithRoles.User.Id);
            return Ok(userWithRoles);
        }

        private async Task<IEnumerable<RoleResponse>> GetUserRolesMappedAsync(Guid userId)
        {
            return _mapper.Map<IEnumerable<RoleResponse>>(await _serviceUserRole.GetUserRolesAsync(ur => ur.UserId == userId));
        }
    }
}
