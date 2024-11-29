using FluentValidation;

namespace UMBIT.ToDo.Dominio.Utilitarios
{
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {
            RuleFor(password => password)
                .NotEmpty().WithMessage("O campo 'Senha' é obrigatório.")
                .MinimumLength(8).WithMessage("Senha deve conter pelo menos 8 caracteres.")
                .Matches(@"[A-Z]").WithMessage("Senha deve conter ao menos uma letra maiúscula.")
                .Matches(@"[a-z]").WithMessage("Senha deve conter ao menos uma letra minúscula.")
                .Matches(@"\d").WithMessage("Senha deve conter ao menos um dígito.")
                .Matches(@"[\W_]").WithMessage("Senha deve conter ao menos um caractere especial.");
        }
    }
}
