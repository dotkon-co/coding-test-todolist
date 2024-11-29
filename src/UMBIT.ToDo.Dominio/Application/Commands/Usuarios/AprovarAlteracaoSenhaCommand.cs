using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.Usuarios
{
    public class AprovarAlteracaoSenhaCommand : UMBITCommand<AprovarAlteracaoSenhaCommand>
    {
        public Guid UsuarioId { get; private set; }

        public AprovarAlteracaoSenhaCommand(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }

        protected override void Validadors(ValidatorCommand<AprovarAlteracaoSenhaCommand> validator)
        {
            validator
                .RuleFor(x => x.UsuarioId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id  do usuário é obrigatório.");
        }
    }
}
