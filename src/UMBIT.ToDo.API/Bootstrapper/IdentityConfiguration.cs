using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UMBIT.ToDo.API.Extensao;
using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;
using UMBIT.ToDo.Dominio.Configuradores;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Infraestrutura.Contextos;

namespace UMBIT.ToDo.API.Bootstrapper
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration, string connectionString = "identity")
        {
            var conexao = configuration.GetConnectionString(connectionString);
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(conexao, b => b.MigrationsAssembly(ProjetoAssemblyHelper.NameProjetoInterface)));

            services.AddIdentity<Usuario, Role>((options) =>
                    {
                    })
                    .AddErrorDescriber<IdentityMensagensPortugues>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders();

            var sectionSettings = configuration.GetSection(nameof(IdentitySettings));

            services.Configure<IdentitySettings>(sectionSettings);
            var identitySettings = sectionSettings.Get<IdentitySettings>() ?? new IdentitySettings();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = identitySettings.RequiredLength;
                options.Password.RequireDigit = identitySettings.RequireDigit;
                options.User.RequireUniqueEmail = identitySettings.RequireUniqueEmail;
                options.Password.RequireLowercase = identitySettings.RequireLowercase;
                options.Password.RequireUppercase = identitySettings.RequireUppercase;
                options.Password.RequiredUniqueChars = identitySettings.RequiredUniqueChars;
                options.Lockout.AllowedForNewUsers = identitySettings.AllowedForNewUsers;
                options.Lockout.MaxFailedAccessAttempts = identitySettings.MaxFailedAccessAttempts;
                options.SignIn.RequireConfirmedEmail = identitySettings.RequireConfirmedEmail;
                options.Password.RequireNonAlphanumeric = identitySettings.RequireNonAlphanumeric;
                options.User.AllowedUserNameCharacters = identitySettings.AllowedUserNameCharacters;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.DefaultLockoutTimeSpan);
            });

            return services;
        }

        public static IApplicationBuilder UseIdentityMigrations(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<IdentityContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    try
                    {
                        context.Database.Migrate();
                        context?.Database.EnsureCreated();
                    }
                    catch (Exception)
                    {
                        if (!context?.Database.EnsureCreated() ?? false)
                            context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }

                }
            }

            return app;
        }
    }
}
