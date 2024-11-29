using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.Dominio.Interfaces;

namespace UMBIT.ToDo.API.Bootstrapper
{
    public static class DependenciasConfigurate
    {
        public static IServiceCollection AddDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));

            return services;
        }
    }
}
