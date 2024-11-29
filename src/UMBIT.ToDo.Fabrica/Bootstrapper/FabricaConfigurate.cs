using Microsoft.AspNetCore.Builder;
using UMBIT.ToDo.BuildingBlocks.Fabrica.Models;

namespace UMBIT.ToDo.BuildingBlocks.Fabrica.Bootstrapper
{
    public static class FabricaConfigurate
    {
        public static IApplicationBuilder UseFabricaGenerica(this IApplicationBuilder app)
        {
            FabricaGenerica.Initialize(app.ApplicationServices);
            return app;
        }
    }
}
