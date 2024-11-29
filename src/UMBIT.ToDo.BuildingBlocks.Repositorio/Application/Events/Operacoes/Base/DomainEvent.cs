
using MediatR;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes.Base
{
    public abstract class DomainEvent : INotification
    {
        public DomainEvent() { }
        public virtual string ObtenhaChaveDeComunicacao() => GetType().Namespace!;
    }
}
