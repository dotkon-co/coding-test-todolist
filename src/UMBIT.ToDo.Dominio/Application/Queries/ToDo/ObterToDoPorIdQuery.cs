using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.Dominio.Application.Queries.ToDo
{
    public class ObterToDoPorIdQuery : UMBITQuery<ObterToDoPorIdQuery, ToDoItem>
    {
        public Guid Id { get; private set; }

        protected ObterToDoPorIdQuery() { }


        public ObterToDoPorIdQuery(Guid id)
        {
            Id = id;
        }

        protected override void Validadors(ValidatorQuery<ObterToDoPorIdQuery> validator)
        {
            validator
                .RuleFor(qry => qry.Id)
                .NotEqual(Guid.Empty).WithMessage("Id de usuário inválido");
        }
    }
}
