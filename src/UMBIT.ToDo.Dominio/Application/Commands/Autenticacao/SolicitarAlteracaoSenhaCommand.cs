using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Commands.Autenticacao
{
    public class SolicitarAlteracaoSenhaCommand : UMBITCommand<SolicitarAlteracaoSenhaCommand>
    {
        public string Email { get; private set; }

        public SolicitarAlteracaoSenhaCommand(string email)
        {
            Email = email;
        }

        protected override void Validadors(ValidatorCommand<SolicitarAlteracaoSenhaCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.Email)
                .SetValidator(new EmailValidator());
        }
    }
}
