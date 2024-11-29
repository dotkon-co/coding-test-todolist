using FluentValidation.Results;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes.Base;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public ValidationResult Validate { get; }

        public DomainEvent ObtenhaEventoAdicao(IBaseEntity data);
        public DomainEvent ObtenhaEventoRemocao(IBaseEntity data);
        public DomainEvent ObtenhaEventoEdicao(IBaseEntity dataOrigin, IBaseEntity dataEdited);
    }
}
