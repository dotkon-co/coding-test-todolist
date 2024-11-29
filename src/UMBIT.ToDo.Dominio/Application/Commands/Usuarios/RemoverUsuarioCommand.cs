using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.Usuarios
{
    public class RemoverUsuarioCommand : UMBITCommand<RemoverUsuarioCommand>
    {
        public Guid IdUsuario { get; private set; }

        protected RemoverUsuarioCommand() { }


        public RemoverUsuarioCommand(Guid idUsuario)
        {
            IdUsuario = idUsuario;
        }
        protected override void Validadors(ValidatorCommand<RemoverUsuarioCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.IdUsuario)
                .NotEqual(Guid.Empty).WithMessage("Id do usuário é obrigatório");
        }
    }
}
