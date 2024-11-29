using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Commands.Autenticacao
{
    public class RemoverTokensDeUsuarioCommand : UMBITCommand<RemoverTokensDeUsuarioCommand>
    {
        public string Email { get; private set; }

        protected RemoverTokensDeUsuarioCommand() { }

        public RemoverTokensDeUsuarioCommand(string email)
        {
            Email = email;
        }

        protected override void Validadors(ValidatorCommand<RemoverTokensDeUsuarioCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.Email)
                .SetValidator(new EmailValidator());
        }
    }
}
