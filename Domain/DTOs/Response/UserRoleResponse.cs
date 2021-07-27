using System;
using System.Collections.Generic;

namespace Domain.DTOs.Response
{
    public class UserRoleResponse
    {
        public UserResponse User { get; set; }
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}
