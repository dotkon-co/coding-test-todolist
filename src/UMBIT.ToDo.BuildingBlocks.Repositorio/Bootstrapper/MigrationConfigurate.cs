using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Bootstrapper
{
    public static class MigrationConfigurate
    {
        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<DbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    try
                    {
                        context.Database.Migrate();
                        context?.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        if (!context?.Database.EnsureCreated() ?? false)
#if DEBUG
                            context.Database.EnsureDeleted();
#endif
                        context.Database.Migrate();

                        throw new Exception("Falha ao executar Migration", ex);
                    }

                }
            }
            return app;
        }
    }
}
