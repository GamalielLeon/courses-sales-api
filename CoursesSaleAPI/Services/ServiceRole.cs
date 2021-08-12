using CoursesSaleAPI.Helpers.ErrorHandler;
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
            role.CreatedAt = DateTime.Now;
            try
            {
                //If save was succeded, return the role created, otherwise return a 500 error.
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded) return await _roleManager.FindByNameAsync(role.Name);
                IdentityError error = result.Errors.First();
                throw new CustomException(error.Code, error.Description, Code.Error500);
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, nameof(Role));
            }
        }

        public override async Task<Role> UpdateAsync(Guid id, Role role)
        {
            Role roleToUpdate = await _repository.GetAsync(id);
            if (roleToUpdate == null) throw CustomExceptionNotFound404;
            roleToUpdate.Name = role.Name;
            roleToUpdate.Code = role.Code;
            roleToUpdate.UpdatedAt = DateTime.Now;

            try
            {
                //If save was successfull, return the role created, otherwise return a 500 error.
                IdentityResult result = await _roleManager.UpdateAsync(roleToUpdate);
                if (result.Succeeded) return await _roleManager.FindByNameAsync(roleToUpdate.Name);
                IdentityError error = result.Errors.First();
                throw new CustomException(error.Code, error.Description, Code.Error500);
            }
            catch (Exception ex)
            {
                throw CheckExceptionforDuplicateValue(ex, nameof(Role));
            }
        }
    }
}
