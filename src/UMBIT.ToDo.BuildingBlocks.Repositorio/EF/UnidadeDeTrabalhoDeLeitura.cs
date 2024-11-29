
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.CompilerServices;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{
    public class UnidadeDeTrabalhoDeLeitura : IUnidadeDeTrabalhoDeLeitura
    {
        private DbContext _contexto { get; set; }
        private IServiceProvider _serviceProvider { get; set; }
        private IDbContextTransaction _transacao { get; set; }
        private INotificador _notificador { get; set; }

        public UnidadeDeTrabalhoDeLeitura(DbContext contexto, IServiceProvider serviceProvider, INotificador notificador)
        {
            _contexto = contexto;
            _notificador = notificador;
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            if (_transacao != null)
            {
                _transacao.Dispose();
            }

            _contexto.Dispose();
        }

        IRepositorioDeLeitura<T> IUnidadeDeTrabalhoDeLeitura.ObterRepositorio<T>()
        {
            return _serviceProvider.GetService(typeof(IRepositorioDeLeitura<T>)) as IRepositorioDeLeitura<T> ?? new RepositorioDeLeitura<T>(_contexto);
        }
    }
}
