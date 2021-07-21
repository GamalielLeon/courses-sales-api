using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace CoursesSaleAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostServer = CreateHostBuilder(args).Build();
            //Create a user only if there are no users in the database.
            using (var environment = hostServer.Services.CreateScope())
            {
                var services = environment.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                if (!userManager.Users.Any())
                {
                    var user = new User { FirstName = "Admin", LastName = "User", UserName = "adminuser", Email = "admin.user@users.com" };
                    userManager.CreateAsync(user, "Password1.").Wait();
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
