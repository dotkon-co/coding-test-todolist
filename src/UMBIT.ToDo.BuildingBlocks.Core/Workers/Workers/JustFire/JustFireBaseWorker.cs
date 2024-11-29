using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UMBIT.ToDo.BuildingBlocks.Core.Workers.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Core.Workers.Workers.JustFire
{
    public abstract class JustFireBaseWorker : BackgroundService, IWorker
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        public JustFireBaseWorker(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

    }
}
