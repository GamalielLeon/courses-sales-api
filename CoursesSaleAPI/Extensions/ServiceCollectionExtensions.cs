using CoursesSaleAPI.Services;
using Domain.Contracts.Service;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesSaleAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IServiceGeneric<>), typeof(ServiceGeneric<>));
            services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
            services.AddScoped<IServiceCourse, ServiceCourse>();
            services.AddScoped<IServiceUser, ServiceUser>();
            services.AddScoped<IServiceRole, ServiceRole>();
            services.AddScoped<IServiceUserRole, ServiceUserRole>();
            return services;
        }
    }
}
