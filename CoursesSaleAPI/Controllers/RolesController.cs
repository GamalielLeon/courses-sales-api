using AutoMapper;
using Domain.Constants;
using Domain.Contracts.Service;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Controllers
{
    [Authorize(Roles = GlobalConstants.ROLES_ALLOWED_FOR_ROLES_CONTROLLER)]
    [Route(GlobalConstants.GENERIC_ENDPOINT)]
    [ApiController]
    public class RolesController : GenericController<Role, RoleRequest, RoleResponse>
    {
        private readonly IServiceRole _serviceRole;
        public RolesController(IServiceRole service, IMapper mapper) : base(service, mapper)
        {
            _serviceRole = (IServiceRole)_service;
        }

        [HttpPost]
        public override async Task<ActionResult<RoleResponse>> PostAsync([FromBody] RoleRequest roleRequest)
        {
            return Created("", _mapper.Map<RoleResponse>(await _serviceRole.AddAsync(_mapper.Map<Role>(roleRequest))));
        }
    }
}
