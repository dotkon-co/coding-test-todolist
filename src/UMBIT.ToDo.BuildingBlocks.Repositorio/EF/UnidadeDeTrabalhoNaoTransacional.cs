
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{
    public class UnidadeDeTrabalhoNaoTransacional : IUnidadeDeTrabalhoNaoTransacional
    {
        private readonly DbContext _contexto;
        private readonly INotificador _notificador;
        private readonly IDbContextTransaction _transacao;
        private readonly IServiceProvider _serviceProvider;

        public UnidadeDeTrabalhoNaoTransacional(DbContext contexto, IServiceProvider serviceProvider, INotificador notificador)
        {
            _contexto = contexto;
            _notificador = notificador;
            _serviceProvider = serviceProvider;
        }

        public async Task<int> SalveAlteracoes()
        {
            return await _contexto.SaveChangesAsync();
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
    }
}
