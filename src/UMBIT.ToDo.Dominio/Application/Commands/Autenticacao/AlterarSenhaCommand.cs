using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Commands.Autenticacao
{
    public class AlterarSenhaCommand : UMBITCommand<AlterarSenhaCommand>
    {
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string ConfirmarSenha { get; private set; }

        protected AlterarSenhaCommand() { }

        public AlterarSenhaCommand(
            string email,
            string senha,
            string confirmarSenha)
        {
            Email = email;
            Senha = senha;
            ConfirmarSenha = confirmarSenha;
        }

        protected override void Validadors(ValidatorCommand<AlterarSenhaCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.Email)
                .SetValidator(new EmailValidator());

            validator
                .RuleFor(cmd => cmd.Senha)
                .SetValidator(new PasswordValidator());

            validator
                .RuleFor(cmd => cmd.ConfirmarSenha)
                .Equal(cmd => cmd.Senha).WithMessage("O campo 'Confirmar Senha' e 'Senha' devem ser iguais.");
        }
    }
}
