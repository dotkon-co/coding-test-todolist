using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Data;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Handler
{
    public class TransactionEventHandler :
        INotificationHandler<BeginTransactionEvent>,
        INotificationHandler<CommitTransactionEvent>,
        INotificationHandler<RollbackTransactionEvent>
    {
        private readonly IMediator Mediator;
        private readonly IUnidadeDeTrabalho UnidadeDeTrabalho;
        private readonly ILogger<TransactionEventHandler> Logger;
        private readonly IRepositorio<TrackEvent> RepositorioTrackEvent;
        public TransactionEventHandler(IUnidadeDeTrabalho unidadeDeTrabalho, IMediator mediator, ILogger<TransactionEventHandler> logger)
        {
            Logger = logger;
            Mediator = mediator;
            UnidadeDeTrabalho = unidadeDeTrabalho;
            RepositorioTrackEvent = unidadeDeTrabalho.ObterRepositorio<TrackEvent>();
        }

        public Task Handle(BeginTransactionEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(CommitTransactionEvent notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Emitindo Eventos de dominio, transação {notification.TransactionId}");

            var trackEvents = await RepositorioTrackEvent.Query().Where(t => t.TransactionId == notification.TransactionId).ToListAsync();
            if (trackEvents.Any())
            {
                foreach (var trackEvent in trackEvents)
                {
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            var assembly = Assembly.Load(trackEvent.AssemblyName);
                            var tipo = assembly.GetType(trackEvent.TypeName);
                            var jOptions = new JsonSerializerOptions()
                            {
                                IncludeFields = true,
                                WriteIndented = true,
                                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                            };
                            var entity = (IBaseEntity)JsonSerializer.Deserialize(trackEvent.DataOriginSerial, tipo!, jOptions)!;

                            switch ((EntityState)trackEvent.StatusEdited)
                            {
                                case EntityState.Added:
                                    Mediator!.Publish(entity!.ObtenhaEventoAdicao(entity), cancellationToken);
                                    break;
                                case EntityState.Modified:
                                    Mediator!.Publish(entity!.ObtenhaEventoEdicao(entity, (IBaseEntity)JsonSerializer.Deserialize(trackEvent.DataEditedSerial, tipo!, jOptions)!), cancellationToken);
                                    break;
                                case EntityState.Deleted:
                                    Mediator!.Publish(entity!.ObtenhaEventoRemocao(entity), cancellationToken);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, $"Erro ao Emitir Eventos de dominio, transação {notification.TransactionId}");
                        }
                    });

                    RepositorioTrackEvent.Remover(trackEvent);

                }

                await UnidadeDeTrabalho.SalveAlteracoes();
            }


        }

        public Task Handle(RollbackTransactionEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
