using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.Autenticacao
{
    public class AtualizarSessaoDeUsuarioCommand : UMBITCommand<AtualizarSessaoDeUsuarioCommand>
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        protected AtualizarSessaoDeUsuarioCommand() { }

        public AtualizarSessaoDeUsuarioCommand(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        protected override void Validadors(ValidatorCommand<AtualizarSessaoDeUsuarioCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.AccessToken)
                .NotEmpty().WithMessage("O campo 'AccessToken' é obrigatório.")
                .NotEmpty().WithMessage("O campo 'RefreshToken' é obrigatório.");
        }
    }
}
