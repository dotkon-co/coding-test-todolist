using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.ToDo
{
    public class AtualizarStatusToDoItemCommand : UMBITCommand<AtualizarStatusToDoItemCommand>
    {
        public Guid Id { get; set; }    
        public int Status { get; set; } 
        protected override void Validadors(ValidatorCommand<AtualizarStatusToDoItemCommand> validator)
        {

            validator.RuleFor((t) => t.Id)
                .NotEmpty()
                .WithMessage("Id Obrigatorio");
        }
    }
}
