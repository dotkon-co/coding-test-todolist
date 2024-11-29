using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using UMBIT.ToDo.BuildingBlocks.Basicos.Utilitarios;
using UMBIT.ToDo.BuildingBlocks.Core.Workers.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Core.Workers.Bootstrapper
{
    public static class WorkersConfigurate
    {
        public static IServiceCollection AddWorkers(this IServiceCollection services, List<Assembly>? CustomRegisterAssemblies = null)
        {
            var assemblies = CustomRegisterAssemblies == null ? ProjetoAssemblyHelper.ObtenhaAppAssemblys() : ProjetoAssemblyHelper.ObtenhaAppAssemblys().Concat(CustomRegisterAssemblies);
            var wWorker = new WorkersWrapped();


            wWorker.AddWorkers(services, assemblies);
            return services;
        }
    }

    class WorkersWrapped
    {
        public IServiceCollection AddWorkers(IServiceCollection services, IEnumerable<Assembly> assembliesToFind)
        {
            var assemblys = assembliesToFind
                                    .Where(a => a.GetTypes().Any(t =>
                                    t.IsAbstract == false &&
                                    t.IsInterface == false &&
                                    t.IsClass == true &&
                                    t.IsAssignableTo(typeof(IWorker))));
            foreach (var assembly in assemblys)
            {
                foreach (var worker in assembly.GetTypes().Where(t => t.IsInterface == false && t.IsAssignableTo(typeof(IWorker))))
                {
                    Type type = typeof(WorkersWrapped);
                    MethodInfo methodInfo = type.GetMethod(nameof(AddHostedService));
                    MethodInfo genericMethod = methodInfo.MakeGenericMethod(worker);
                    genericMethod.Invoke(this, new object[] { services });
                }
            }

            return services;
        }
        public IServiceCollection AddHostedService<THostedService>(IServiceCollection services)
            where THostedService : class, IHostedService =>
                services.AddHostedService<THostedService>();
    }
}
