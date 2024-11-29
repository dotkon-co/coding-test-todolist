using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.ToDo
{
    public class DeleteToDoItemCommand : UMBITCommand<DeleteToDoItemCommand>
    {
        public Guid Id { get; set; }
        protected override void Validadors(ValidatorCommand<DeleteToDoItemCommand> validator)
        {
            validator
                .RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id é obrigatório.");
        }
    }
}
