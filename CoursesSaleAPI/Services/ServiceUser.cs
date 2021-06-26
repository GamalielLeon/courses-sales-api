using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Constants;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.DTOs.Request;
using Domain.DTOs.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Security.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceUser : ServiceGeneric<User>, IServiceUser
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private const string UNAUTHORIZED_ERROR = ConstantsErrors.UNAUTHORIZED;

        public ServiceUser(IGenericRepository<User> repository, IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator) : base(repository, unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<User> AddUserAsync(User user, string password)
        {
            //If email or username sent already exists, return a 400 error.
            if (await _repository.AnyAsync(u => u.Email == user.Email))
                throw new CustomException(ConstantsErrors.DUPLICATED_EMAIL, errorDescriptions[ConstantsErrors.DUPLICATED_EMAIL]);
            if (await _repository.AnyAsync(u => u.UserName == user.UserName))
                throw new CustomException(ConstantsErrors.DUPLICATED_USERNAME, errorDescriptions[ConstantsErrors.DUPLICATED_USERNAME]);
            user.CreatedAt = DateTime.Now;
            //If save was succeded, return the user created, otherwise return a 400 error.
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded) return await _userManager.FindByEmailAsync(user.Email);
            IdentityError error = result.Errors.First();
            throw new CustomException(error.Code, error.Description, Code.Error400);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            User user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
                throw new CustomException(UNAUTHORIZED_ERROR, errorDescriptions[UNAUTHORIZED_ERROR], Code.Error401);
            //Check if password sent in the request matches with the password registered.
            if (!(await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false)).Succeeded)
                throw new CustomException(UNAUTHORIZED_ERROR, errorDescriptions[UNAUTHORIZED_ERROR], Code.Error401);
            return new LoginResponse() { Email = user.Email, UserName = user.UserName, Token = CreateToken(user) };
        }

        public async Task<User> GetCurrentUserAsync(string token)
        {
            string email = _jwtGenerator.GetEmailFromToken(token);
            return await _userManager.FindByEmailAsync(email);
        }

        public string CreateToken(User user) => _jwtGenerator.CreateToken(user);
    }
}
