using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;

namespace UMBIT.ToDo.Dominio.Application.Queries.Usuarios
{
    public class ObterUsuarioPorIdQuery : UMBITQuery<ObterUsuarioPorIdQuery, Usuario>
    {
        public Guid Id { get; private set; }

        protected ObterUsuarioPorIdQuery() { }


        public ObterUsuarioPorIdQuery(Guid id)
        {
            Id = id;
        }

        protected override void Validadors(ValidatorQuery<ObterUsuarioPorIdQuery> validator)
        {
            validator
                .RuleFor(qry => qry.Id)
                .NotEqual(Guid.Empty).WithMessage("Id de usuário inválido");
        }
    }
}
