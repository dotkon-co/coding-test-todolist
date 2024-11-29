using Microsoft.Extensions.DependencyInjection;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;

namespace UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Bootstrapper
{
    public static class SegurancaConfigurate
    {
        public static IServiceCollection AddContextPrincipal(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ContextoPrincipal>();

            return services;
        }
    }
}
