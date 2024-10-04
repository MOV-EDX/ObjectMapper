using Microsoft.Extensions.DependencyInjection;
using ObjectMapper.Mappers;
using ObjectMapper.Mappers.Interfaces;

namespace ObjectMapper.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddObjectMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper, RecursiveMap>();
            return services;
        }
    }
}
