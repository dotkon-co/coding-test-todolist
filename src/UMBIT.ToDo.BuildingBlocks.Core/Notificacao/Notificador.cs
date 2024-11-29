using FluentValidation.Results;
using UMBIT.ToDo.BuildingBlocks.Basicos.Notificacoes;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Core.Notificacao
{
    public class Notificador : INotificador
    {
        private readonly List<NotificacaoPadrao> _notificacoes;
        private readonly List<ErroSistema> _errosSistema;


        public Notificador()
        {
            _notificacoes = new List<NotificacaoPadrao>();
            _errosSistema = new List<ErroSistema>();
        }

        public IEnumerable<NotificacaoPadrao> ObterTodos()
        {
            var colecao = new List<NotificacaoPadrao>();

            colecao.AddRange(_notificacoes);
            colecao.AddRange(_errosSistema);

            return colecao;
        }

        public IEnumerable<NotificacaoPadrao> ObterNotificacoes()
            => _notificacoes;


        public IEnumerable<ErroSistema> ObterErrosSistema()
            => _errosSistema;

        public void AdicionarNotificacao(NotificacaoPadrao notificacao)
            => _notificacoes.Add(notificacao);

        public void AdicionarNotificacao(ValidationResult validationResult)
        {
            if (validationResult != null && !validationResult.IsValid)
                foreach (var erro in validationResult.Errors)
                {
                    _notificacoes.Add(new NotificacaoPadrao(erro.PropertyName, erro.ErrorMessage));
                }
        }

        public void AdicionarErroSistema(ErroSistema erroSistema)
            => _errosSistema.Add(erroSistema);

        public bool TemNotificacoes()
        {
            if (_notificacoes.Count > 0 || _errosSistema.Count > 0)
                return true;

            return false;
        }

        public void LimparNotificacoes()
        {
            _notificacoes.Clear();
            _errosSistema.Clear();
        }

        public void AdicionarNotificacao(string mensagem)
        {
            _notificacoes.Add(new NotificacaoPadrao(mensagem));
        }

        public void AdicionarNotificacao(string titulo, string mensagem)
        {
            _notificacoes.Add(new NotificacaoPadrao(titulo, mensagem));
        }
    }
}
