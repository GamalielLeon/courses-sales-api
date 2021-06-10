using Domain.Contracts.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class UnitOfWorkCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
