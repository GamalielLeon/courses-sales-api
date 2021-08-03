using Domain.DTOs.Pagination;
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
        Task<User> GetCurrentUserAsync(string token);
        string CreateToken(User user, string[] roles = null);
    }
}
