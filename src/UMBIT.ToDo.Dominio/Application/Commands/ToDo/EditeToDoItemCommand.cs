using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.ToDo
{
    public class EditeToDoItemCommand : UMBITCommand<EditeToDoItemCommand>
    {
        public Guid Id { get; set; }
        public Guid? IdToDoList { get; set; }
        public int Index { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Status { get; set; }

        protected override void Validadors(ValidatorCommand<EditeToDoItemCommand> validator)
        {
            validator
                .RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id é obrigatório.");

            validator.RuleFor(x => x.Nome)
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("O nome deve conter apenas letras.")
            .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");

            validator.RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

            validator.RuleFor(x => x.DataFim)
                .GreaterThan(x => x.DataInicio).WithMessage("A data de fim deve ser posterior à data de início.");

            validator.RuleFor(x => x.IdToDoList)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("Vincule a uma Lista Valida");
        }
    }
}
