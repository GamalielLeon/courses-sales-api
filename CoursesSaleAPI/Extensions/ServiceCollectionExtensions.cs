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
            services.AddScoped<IServiceCourse, ServiceCourse>();
            return services;
        }
    }
}
