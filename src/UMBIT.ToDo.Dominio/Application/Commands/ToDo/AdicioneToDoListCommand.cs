using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.ToDo
{
    public class AdicioneToDoListCommand : UMBITCommand<AdicioneToDoListCommand>
    {
        public string Nome { get; set; }
        public List<(string Nome, string? Descricao, DateTime DataInicio, DateTime DataFim, int Status)>? Items { get;set; }
        protected override void Validadors(ValidatorCommand<AdicioneToDoListCommand> validator)
        {
            validator.RuleFor(x => x.Nome)
                     .Matches(@"^[a-zA-Z\s]+$").WithMessage("O nome deve conter apenas letras.")
                     .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");

           validator.RuleForEach(x => x.Items)
                    .ChildRules(items =>
                    {
                        items.RuleFor(item => item.Nome)
                            .Matches(@"^[a-zA-Z\s]+$").WithMessage("O nome do item deve conter apenas letras.")
                            .MaximumLength(50).WithMessage("O nome do item deve ter no máximo 50 caracteres.");

                        items.RuleFor(item => item.Descricao)
                            .MaximumLength(500).WithMessage("A descrição do item deve ter no máximo 500 caracteres.");

                        items.RuleFor(item => item.DataFim)
                            .GreaterThan(item => item.DataInicio).WithMessage("A data de fim do item deve ser posterior à data de início.");
                    });
        }
    }
}
