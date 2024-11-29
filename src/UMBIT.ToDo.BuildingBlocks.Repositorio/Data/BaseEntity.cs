using FluentValidation;
using FluentValidation.Results;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes.Base;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Data
{
    public abstract class BaseEntity<T> : IBaseEntity where T : BaseEntity<T>
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        private Validator<T> Validator { get; set; }
        public ValidationResult Validate
        {
            get
            {
                {
                    return Validator.Validate(this as T);
                }
            }
        }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            Validator = new Validator<T>();

            Validadors(Validator);
        }

        protected abstract void Validadors(Validator<T> validator);

        public virtual DomainEvent ObtenhaEventoAdicao(IBaseEntity data)
        {
            return new BaseEntityAddedEvent(data);
        }
        public virtual DomainEvent ObtenhaEventoRemocao(IBaseEntity data)
        {
            return new BaseEntityRemovedEvent(data);
        }
        public virtual DomainEvent ObtenhaEventoEdicao(IBaseEntity dataOrigin, IBaseEntity dataEdited)
        {
            return new BaseEntityEditedEvent(dataOrigin, dataEdited);
        }

        public class BaseEntityAddedEvent : BaseEntityAddedEventBase<T>
        {
            public BaseEntityAddedEvent(IBaseEntity data) : base(data)
            {
            }
        }
        public class BaseEntityEditedEvent : BaseEntityEditedEventBase<T>
        {
            public BaseEntityEditedEvent(IBaseEntity dataOrigin, IBaseEntity dataEdited) : base(dataOrigin, dataEdited)
            {
            }

        }
        public class BaseEntityRemovedEvent : BaseEntityRemovedEventBase<T>
        {
            public BaseEntityRemovedEvent(IBaseEntity data) : base(data)
            {
            }
        }
    }
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public virtual ValidationResult Validate
        {
            get
            {
                {
                    return new ValidationResult();
                }
            }
        }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public abstract DomainEvent ObtenhaEventoAdicao(IBaseEntity data);

        public abstract DomainEvent ObtenhaEventoRemocao(IBaseEntity data);

        public abstract DomainEvent ObtenhaEventoEdicao(IBaseEntity dataOrigin, IBaseEntity dataEdited);
    }
    public class Validator<T> : AbstractValidator<T> where T : class
    {
        public Validator()
        {
        }
    }
}
