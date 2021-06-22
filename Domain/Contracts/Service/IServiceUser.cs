using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Contracts.Service
{
    public interface IServiceUser : IServiceGeneric<User>
    {
        Task<User> AddUserAsync(User user, string password);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        string CreateToken(User user);
    }
}
