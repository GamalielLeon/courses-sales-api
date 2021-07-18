using Domain.DTOs.Pagination;
using Domain.Entities;

namespace Domain.Contracts.Service
{
    public interface IServiceRole : IServiceGeneric<Role, RolesPaged>
    {
    }
}
