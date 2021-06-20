using Domain.DTOs.Request;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceUser : IServiceGeneric<User>
    {
        Task<User> LoginAsync(LoginRequest loginRequest);
    }
}
