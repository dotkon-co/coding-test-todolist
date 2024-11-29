using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.Dominio.Application.Queries.ToDo
{
    public class ObterToDoListPorIdQuery : UMBITQuery<ObterToDoListPorIdQuery, ToDoList>
    {
        public Guid Id { get; private set; }

        protected ObterToDoListPorIdQuery() { }


        public ObterToDoListPorIdQuery(Guid id)
        {
            Id = id;
        }

        protected override void Validadors(ValidatorQuery<ObterToDoListPorIdQuery> validator)
        {
            validator
                .RuleFor(qry => qry.Id)
                .NotEqual(Guid.Empty).WithMessage("Id de usuário inválido");
        }
    }
}
