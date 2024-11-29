
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MediatorBus;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus.Models;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Message.Bootstrapper
{
    public static class MessageConfigurate
    {
        public static IServiceCollection AddMessages(this IServiceCollection services, IConfiguration configuration, List<Assembly>? CurtomRegisterAssemblies = null)
        {
            var appAssemblies = ProjetoAssemblyHelper.ObtenhaAppAssemblys().Append(typeof(MessageConfigurate).Assembly).Append(typeof(IUnidadeDeTrabalho).Assembly)!;

            appAssemblies = CurtomRegisterAssemblies == null ? appAssemblies : appAssemblies.Concat(CurtomRegisterAssemblies);

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(appAssemblies.ToArray()));
            services.AddTransient<IMediatorBus, MediatorBus>();

            services.Configure<UMBITMessageBusConfig>(configuration.GetSection("UMBITMessageBusConfig"));


            services.AddSingleton<UMBITMessageBusConnectionFactory>();
            services.AddSingleton<IMessagerBus, MessagerBus>();

            return services;
        }
    }
}
