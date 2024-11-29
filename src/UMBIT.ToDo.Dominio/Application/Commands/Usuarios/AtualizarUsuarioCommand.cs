using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.Usuarios
{
    public class AtualizarUsuarioCommand : UMBITCommand<AtualizarUsuarioCommand>
    {
        public Guid IdUsuario { get; private set; }
        public string NomeUsuario { get; private set; }

        public AtualizarUsuarioCommand() { }

        public AtualizarUsuarioCommand(Guid idUsuario, string nomeUsuario)
        {
            IdUsuario = idUsuario;
            NomeUsuario = nomeUsuario;
        }

        protected override void Validadors(ValidatorCommand<AtualizarUsuarioCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.IdUsuario)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do usuáiro inválido");

            validator
                .RuleFor(cmd => cmd.NomeUsuario)
                .NotEmpty()
                .WithMessage("Nome do usuário é obrigatório");
        }
    }
}
