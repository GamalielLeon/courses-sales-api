using CoursesSaleAPI.Helpers.ErrorHandler;
using Domain.Constants;
using Domain.Contracts.Repository;
using Domain.Contracts.Service;
using Domain.Contracts.UnitOfWork;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI.Services
{
    public class ServiceRole : ServiceGeneric<Role>, IServiceRole
    {
        private readonly RoleManager<Role> _roleManager;

        public ServiceRole(RoleManager<Role> roleManager, IGenericRepository<Role> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
            _roleManager = roleManager;
        }

        public override async Task<Role> AddAsync(Role role)
        {
            //If code or name sent already exists, return a 400 error.
            if (await _repository.AnyAsync(r => r.NormalizedName == role.Name.ToUpper()))
                throw new CustomException(ConstantsErrors.DUPLICATED_NAME, errorDescriptions[ConstantsErrors.DUPLICATED_NAME]);
            if (await _repository.AnyAsync(r => r.Code.ToUpper() == role.Code.ToUpper()))
                throw new CustomException(ConstantsErrors.DUPLICATED_CODE, errorDescriptions[ConstantsErrors.DUPLICATED_CODE]);
            role.CreatedAt = DateTime.Now;
            //If save was succeded, return the role created, otherwise return a 500 error.
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded) return await _roleManager.FindByNameAsync(role.Name);
            IdentityError error = result.Errors.First();
            throw new CustomException(error.Code, error.Description, Code.Error500);
        }
    }
}
