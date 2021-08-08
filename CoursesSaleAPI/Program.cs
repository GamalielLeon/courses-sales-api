using Domain.Constants;
using Domain.Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesSaleAPI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var hostServer = CreateHostBuilder(args).Build();
            //Create a user only if there are no users in the database.
            using (var environment = hostServer.Services.CreateScope())
            {
                var services = environment.ServiceProvider;
                UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
                RoleManager<Role> roleManager = services.GetRequiredService<RoleManager<Role>>();
                var context = services.GetService<OnlineCoursesContext>();
                GenericRepository<UserRole> userRoleRepository = new GenericRepository<UserRole>(context);
                User user = new User { FirstName = "The Main", LastName = "Administrator User", UserName = "first01superadmin", Email = "superadmin.admins@email.com", CreatedAt = DateTime.Now };
                Role role = new Role { Name = "Super Administrator", Code = GlobalConstants.SUPER_ADMIN_ROLE, CreatedAt = DateTime.Now };

                if (!userManager.Users.Any()) await userManager.CreateAsync(user, ".Ealg-1996");
                if (!roleManager.Roles.Any())
                {
                    await roleManager.CreateAsync(role);
                    Guid userId = (await userManager.FindByNameAsync(user.UserName)).Id;
                    Guid roleId = (await roleManager.FindByNameAsync(role.Name)).Id;
                    await userRoleRepository.AddAsync(new UserRole { UserId = userId, RoleId = roleId, CreatedAt = DateTime.Now });
                    await context.SaveChangesAsync();
                }
            }
            hostServer.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
