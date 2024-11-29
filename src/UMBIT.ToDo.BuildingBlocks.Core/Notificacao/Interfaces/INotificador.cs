using FluentValidation.Results;
using UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes;

namespace UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces
{
    public interface INotificador
    {
        IEnumerable<NotificacaoPadrao> ObterTodos();
        IEnumerable<NotificacaoPadrao> ObterNotificacoes();
        IEnumerable<ErroSistema> ObterErrosSistema();
        void AdicionarNotificacao(string mensagem);
        void AdicionarNotificacao(string titulo, string mensagem);
        void AdicionarNotificacao(ValidationResult validationResult);
        void AdicionarErroSistema(ErroSistema erroSistema);
        bool TemNotificacoes();
        void LimparNotificacoes();
    }
}
