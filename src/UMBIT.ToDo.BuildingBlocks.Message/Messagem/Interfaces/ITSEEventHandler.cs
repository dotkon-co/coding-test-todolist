using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Events;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes.Base;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces
{
    public interface IUMBITEventHandler<T> : INotificationHandler<T> where T : UMBITEvent
    {
    }
    public interface IUMBITDomainEventHandler<T> : INotificationHandler<T> where T : DomainEvent
    {
    }
}
