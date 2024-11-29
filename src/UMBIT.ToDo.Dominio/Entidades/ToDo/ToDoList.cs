using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Data;

namespace UMBIT.ToDo.Dominio.Entidades.ToDo
{
    public class ToDoList : BaseEntity<ToDoList>
    {
        public string? Nome { get; set; }
        public Guid IdUsuario { get; set; }
        public string NomeUsuario { get; set; }

        public virtual List<ToDoItem> ToDoItems { get; set; }

        protected override void Validadors(Validator<ToDoList> validator)
        {
            validator
                .RuleFor(x => x.IdUsuario)
                .NotNull()
                .NotEmpty()
                .WithMessage("O Id de Usuario é Obrigatorio!");

            validator
                .RuleFor(x => x.NomeUsuario)
                .NotNull()
                .NotEmpty()
                .WithMessage("O Nome de Usuario é Obrigatorio!");

            validator.RuleFor(x => x.Nome)
                     .Matches(@"^[a-zA-Z\s]+$").WithMessage("O nome deve conter apenas letras.")
                     .MaximumLength(50).WithMessage("O nome deve ter no máximo 50 caracteres.");

        }
    }
}
