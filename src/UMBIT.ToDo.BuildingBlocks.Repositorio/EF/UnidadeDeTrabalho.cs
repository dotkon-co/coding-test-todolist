
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Application.Events.Operacoes;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        private DbContext _contexto { get; set; }
        private IDbContextTransaction _transacao;
        private readonly INotificador _notificador;
        private readonly IServiceProvider _serviceProvider;
        private bool _transacaoAberta { get; set; }

        public UnidadeDeTrabalho(DbContext contexto, IServiceProvider serviceProvider, INotificador notificador)
        {
            _contexto = contexto;
            _notificador = notificador;
            _serviceProvider = serviceProvider;
        }

        public async Task<int> SalveAlteracoes()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task InicieTransacao()
        {
            if (!_transacaoAberta)
            {
                _transacao = await _contexto.Database.BeginTransactionAsync();
                _transacaoAberta = true;

                _ = EnvieEvento(new BeginTransactionEvent(_transacao.TransactionId));
            }
        }

        public async Task FinalizeTransacao()
        {
            if (_transacaoAberta)
            {
                var idTransaction = _transacao.TransactionId;

                await _transacao.CommitAsync();
                await _transacao.DisposeAsync();
                _transacaoAberta = false;

                _ = EnvieEvento(new CommitTransactionEvent(idTransaction));

            }
        }

        public async Task RevertaTransacao()
        {
            if (_transacaoAberta)
            {
                var idTransaction = _transacao.TransactionId;

                await _transacao.RollbackAsync();
                await _transacao.DisposeAsync();
                _transacaoAberta = false;
                _ = EnvieEvento(new RollbackTransactionEvent(idTransaction));
            }
        }

        public void Dispose()
        {
            if (_transacao != null)
            {
                _transacao.Dispose();
            }

            _contexto.Dispose();
        }

        public IRepositorio<T> ObterRepositorio<T>() where T : class
        {
            return _serviceProvider.GetService(typeof(IRepositorio<T>)) as IRepositorio<T> ?? new Repositorio<T>(_contexto, _notificador);
        }

        private Task EnvieEvento(INotification notification) => Task.Run(() => _serviceProvider.CreateScope().ServiceProvider.GetService<IMediator>()!.Publish(notification));
    }
}
