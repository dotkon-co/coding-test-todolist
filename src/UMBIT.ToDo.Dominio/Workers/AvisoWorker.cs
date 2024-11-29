using Microsoft.Extensions.DependencyInjection;
using UMBIT.ToDo.BuildingBlocks.Core.Workers.Workers.JustFire;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.SignalR.Interfaces;

namespace UMBIT.ToDo.Dominio.Workers
{
    public class AvisoWorker : JustFireBaseWorker
    {
        private readonly IMessagerBus _messagerBus;
        public AvisoWorker(IServiceScopeFactory serviceScopeFactory, IMessagerBus messagerBus) : base(serviceScopeFactory)
        {
            _messagerBus = messagerBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messagerBus.Subscribe<AvisoMessage>((t) =>
            {
                var signalClient = ServiceScopeFactory.CreateScope().ServiceProvider.GetService<ISignalRClient>();

                signalClient.EmitaAtualizacao("avisos", "Faça sua Tarefa", t.IdUsuario.ToString());

            });

            return Task.CompletedTask;
        }

        public class AvisoMessage : UMBITMensagem
        {
            public AvisoMessage()
            {

            }
            public AvisoMessage(Guid idUsuario)
            {
                IdUsuario = idUsuario;
            }

            public Guid IdUsuario { get; set; }

        }
    }
}
