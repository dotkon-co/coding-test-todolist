using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.Dominio.Application.Commands.ToDo
{
    public class EditeToDoListItemCommand : UMBITCommand<EditeToDoListItemCommand>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        protected override void Validadors(ValidatorCommand<EditeToDoListItemCommand> validator)
        {
            validator
                .RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id é obrigatório.");

            validator.RuleFor(x => x.Nome)
                     .Matches(@"^[a-zA-Z\s]+$").WithMessage("O nome deve conter apenas letras.")
                     .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");
        }
    }
}
