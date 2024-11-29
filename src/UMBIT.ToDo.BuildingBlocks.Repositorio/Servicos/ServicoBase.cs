using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Servicos
{
    public abstract class ServicoBase<T> : IDisposable, IServicoBase<T> where T : class
    {
        protected readonly INotificador Notificador;

        protected readonly IUnidadeDeTrabalho UnidadeDeTrabalho;
        protected IRepositorio<T> Repositorio { get; private set; }

        public ServicoBase(IUnidadeDeTrabalho unidadeDeTrabalho, INotificador notificador)
        {
            Notificador = notificador;
            UnidadeDeTrabalho = unidadeDeTrabalho;
            Repositorio = UnidadeDeTrabalho.ObterRepositorio<T>();
        }

        public virtual async Task AdicionarAsync(T objeto)
        {
            await Repositorio.Adicionar(objeto);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public virtual async Task AdicionarAsync(List<T> objetos)
        {
            await Repositorio.AdicionarTodos(objetos);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public virtual async Task AtualizeAsync(T objeto)
        {
            Repositorio.Atualizar(objeto);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public void Dispose()
        {
            UnidadeDeTrabalho.Dispose();
        }

        public IQueryable<T> Query()
        {
            return Repositorio.Query();
        }
    }

    public abstract class ServicoBase : IDisposable
    {
        protected readonly IUnidadeDeTrabalho UnidadeDeTrabalho;

        public ServicoBase(IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            UnidadeDeTrabalho = unidadeDeTrabalho;
        }

        public void Dispose()
        {
            UnidadeDeTrabalho.Dispose();
        }
    }
}
