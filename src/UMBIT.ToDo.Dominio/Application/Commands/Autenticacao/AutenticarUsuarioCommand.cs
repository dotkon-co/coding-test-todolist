using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Commands.Autenticacao
{
    public class AutenticarUsuarioCommand : UMBITCommand<AutenticarUsuarioCommand>
    {
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string Audience { get; private set; }

        protected AutenticarUsuarioCommand() { }

        public AutenticarUsuarioCommand(string email, string senha, string audience)
        {
            Email = email;
            Senha = senha;
            Audience = audience;
        }

        protected override void Validadors(ValidatorCommand<AutenticarUsuarioCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.Email)
                .SetValidator(new EmailValidator());

            validator
                .RuleFor(cmd => cmd.Senha)
                .SetValidator(new PasswordValidator());

            validator
                .RuleFor(cmd => cmd.Audience)
                .NotEmpty().WithMessage("Audience é obrigatório");
        }
    }
}
