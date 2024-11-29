using FluentValidation;

namespace UMBIT.ToDo.Dominio.Utilitarios
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(email => email)
                .NotEmpty().WithMessage("O campo 'Email' é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");
        }
    }
}
