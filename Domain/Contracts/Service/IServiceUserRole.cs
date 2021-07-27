using Domain.DTOs.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceUserRole : IServiceGeneric<UserRole>
    {
        Task<User> AddRolesToUserAsync(UserRoleRequest userRoleRequest);
        Task<IEnumerable<Role>> GetUserRolesAsync(Expression<Func<UserRole, bool>> predicate);
        Task<User> RemoveRolesFromUserAsync(UserRoleRequest userRoleRequest);
    }
}
