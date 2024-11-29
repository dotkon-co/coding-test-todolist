using System.ComponentModel;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes.Base;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes
{
    public class BaseEntityRemovedEventBase<T> : DomainEvent where T : class
    {
        public BaseEntityRemovedEventBase()
        {

        }
        public BaseEntityRemovedEventBase(IBaseEntity data)
        {
            Data = (data as T)!;
        }

        public T Data { get; set; }

        public override string ObtenhaChaveDeComunicacao()
        {
            return $"{typeof(T).Name}.Removed";
        }
    }
}
