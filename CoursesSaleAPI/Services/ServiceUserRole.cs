using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.DTOs.Request;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceUserRole : ServiceGeneric<UserRole>, IServiceUserRole
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        public ServiceUserRole(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository, IGenericRepository<UserRole> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User> AddRolesToUserAsync(UserRoleRequest userRoleRequest)
        {
            User user = await GetUserByUsernameWithRolesAsync(userRoleRequest.UserName);
            string[] roleCodes = userRoleRequest.RoleCodes.ToArray();
            int roleCodesLength = roleCodes.Length;

            if (roleCodesLength == 0) throw new CustomException("EmptyRoleCodes", "RoleCodes field must not be null or empty");
            if (roleCodes.Distinct().Count() != roleCodesLength)
                throw new CustomException("RoleCodesDuplicate", "One or more role codes are duplicated");
            if (roleCodesLength > 5 || roleCodesLength > await _roleRepository.CountRecordsAsync())
                throw new CustomException("ExceededRoles", "RoleCodes length is greater than 5 or greater than the number of roles in the database");

            IQueryable<Guid> roleIdsMatched = await GetRoleIdsMatchedAsync(roleCodes);
            if (user.UserRoles.Select(static ur => ur.RoleId).Intersect(roleIdsMatched).Any())
                throw new CustomException("RoleAlreadyExists", "This user already has one or more of the submitted roles");

            foreach(Guid roleId in await roleIdsMatched.ToListAsync())
            {
                await _repository.AddAsync(new UserRole() { RoleId = roleId, UserId = user.Id, CreatedAt = DateTime.Now });
            }

            try
            {
                await _unitOfWork.SaveAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw CheckExceptionforDuplicateValue(ex, nameof(Course));
            }
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Expression<Func<UserRole, bool>> predicate)
        {
            return (await _repository.FindByIncludingAsync(predicate, static ur => ur.Role)).Select(static ur => ur.Role);
        }

        public async Task<User> RemoveRolesFromUserAsync(UserRoleRequest userRoleRequest)
        {
            User user = await GetUserByUsernameWithRolesAsync(userRoleRequest.UserName);
            Guid[] userRoles = user.UserRoles.Select(static ur => ur.RoleId).ToArray();
            string[] roleCodes = userRoleRequest.RoleCodes.ToArray();
            int roleCodesLength = roleCodes.Length;

            if (userRoles.Length == 0) throw new CustomException("NoRolesFound", "This user has no roles");
            if (roleCodesLength == 0 || roleCodesLength > userRoles.Length)
                throw new CustomException("RoleCodesLengthError", $"RoleCodes length must be greater than 0 and less than or equal to {userRoles.Length}");
            if (roleCodes.Distinct().Count() != roleCodesLength)
                throw new CustomException("RoleCodesDuplicate", "One or more role codes are duplicated");
            
            IQueryable<Guid> roleIdsMatched = await GetRoleIdsMatchedAsync(roleCodes);
            if (userRoles.Intersect(roleIdsMatched).Count() != roleCodesLength)
                throw new CustomException("RoleNotMatch", "This user does not have one or more of the submitted roles");

            try
            {
                _repository.DeleteRange(await _repository.FindByAsync(ur => ur.UserId == user.Id && roleIdsMatched.Contains(ur.RoleId)));
                await _unitOfWork.SaveAsync();
                return user;
            }
            catch (DbUpdateException ex)
            {
                throw CheckExceptionforDuplicateValue(ex, nameof(Course));
            }
        }

        private async Task<User> GetUserByUsernameWithRolesAsync(string username)
        {
            User user =  await _userRepository.FindOneIncludingAsync(u => u.NormalizedUserName == username, static u => u.UserRoles);
            if (user == null) throw new CustomException("UserNotFound", "This username does not exist", Code.Error404);
            return user;
        }

        private async Task<IQueryable<Guid>> GetRoleIdsMatchedAsync(string[] roleCodes)
        {
            IQueryable<Guid> roleIdsMatched = _roleRepository.FindBy(r => roleCodes.Contains(r.Code)).Select(static r => r.Id);
            if (await roleIdsMatched.CountAsync() != roleCodes.Length)
                throw new CustomException("RoleNotFound", "One or more roles were not found", Code.Error404);
            return roleIdsMatched;
        }
    }
}
