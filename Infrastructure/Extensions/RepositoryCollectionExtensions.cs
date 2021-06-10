using Domain.Contracts.Repository;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
