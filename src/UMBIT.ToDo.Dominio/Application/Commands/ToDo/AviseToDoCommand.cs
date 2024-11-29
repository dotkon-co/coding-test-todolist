using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.ToDo
{
    public class AviseToDoCommand : UMBITCommand<AviseToDoCommand>
    {
        public Guid IdUsuario { get; set; }
        protected override void Validadors(ValidatorCommand<AviseToDoCommand> validator)
        {
            validator
                .RuleFor(x => x.IdUsuario)
                .NotEqual(Guid.Empty)
                .WithMessage("Id é obrigatório.");
        }
    }
}
