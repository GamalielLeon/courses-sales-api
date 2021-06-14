using AutoMapper;
using CoursesSaleAPI.Helpers.Automapper;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesSaleAPI.Extensions
{
    public static class AutomapperCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(static mc => mc.AddProfile(new MappingProfile()));
            IMapper mapper = mapperConfig.CreateMapper();
            return services.AddSingleton(mapper);
        }
    }
}
