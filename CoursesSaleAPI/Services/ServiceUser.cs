using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Constants;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.DTOs.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceUser : ServiceGeneric<User>, IServiceUser
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private const string UNAUTHORIZED_ERROR = ConstantsErrors.UNAUTHORIZED;
        public ServiceUser(IGenericRepository<User> repository, IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager) : base(repository, unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            User user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                throw new CustomException(UNAUTHORIZED_ERROR, errorDescriptions[UNAUTHORIZED_ERROR], Code.Error401);
            //Check if password sent in the request matches with the password registered.
            if (!(await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false)).Succeeded)
                throw new CustomException(UNAUTHORIZED_ERROR, errorDescriptions[UNAUTHORIZED_ERROR], Code.Error401);
            return user;
        }
    }
}
