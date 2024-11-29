using FluentValidation;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.Dominio.Utilitarios;

namespace UMBIT.ToDo.Dominio.Application.Commands.Autenticacao
{
    public class CadastrarUsuarioCommand : UMBITCommand<CadastrarUsuarioCommand>
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public string ConfirmarSenha { get; private set; }

        protected CadastrarUsuarioCommand() { }

        public CadastrarUsuarioCommand(
            string nome,
            string email,
            string senha,
            string confirmarSenha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            ConfirmarSenha = confirmarSenha;
        }

        protected override void Validadors(ValidatorCommand<CadastrarUsuarioCommand> validator)
        {
            validator
                .RuleFor(cmd => cmd.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Must(nome => !string.IsNullOrEmpty(nome) && nome.Split(' ').Length >= 2).WithMessage("Nome deve conter ao menos nome e sobrenome.");

            validator
                .RuleFor(cmd => cmd.Email)
                .SetValidator(new EmailValidator());

            validator
                .RuleFor(cmd => cmd.Senha)
                .SetValidator(new PasswordValidator());

            validator
                .RuleFor(cmd => cmd.ConfirmarSenha)
                .Equal(cmd => cmd.Senha).WithMessage("O campo 'Confirmar Senha' e 'Senha' devem ser iguais.");
        }
    }
}
