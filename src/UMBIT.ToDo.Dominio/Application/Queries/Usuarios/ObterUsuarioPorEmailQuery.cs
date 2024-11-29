using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Queries.Usuarios
{
    public class ObterUsuarioPorEmailQuery : UMBITQuery<ObterUsuarioPorEmailQuery, Usuario>
    {
        public string Email { get; private set; }

        protected ObterUsuarioPorEmailQuery() { }

        public ObterUsuarioPorEmailQuery(string email)
        {
            Email = email;
        }

        protected override void Validadors(ValidatorQuery<ObterUsuarioPorEmailQuery> validator)
        {
            validator
                .RuleFor(qry => qry.Email)
                .SetValidator(new EmailValidator());
        }
    }
}
