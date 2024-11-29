using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.Auth.Token;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Queries.Tokens
{
    public class ObterTokenDeUsuarioQuery : UMBITQuery<ObterTokenDeUsuarioQuery, TokenResult>
    {
        public string Email { get; protected set; }

        protected ObterTokenDeUsuarioQuery() { }

        public ObterTokenDeUsuarioQuery(string email)
        {
            Email = email;
        }

        protected override void Validadors(ValidatorQuery<ObterTokenDeUsuarioQuery> validator)
        {
            validator
                .RuleFor(query => query.Email)
                .SetValidator(new EmailValidator());
        }
    }
}
