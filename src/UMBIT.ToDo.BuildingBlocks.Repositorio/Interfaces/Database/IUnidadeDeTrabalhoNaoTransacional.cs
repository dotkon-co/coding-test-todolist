using System.Runtime.CompilerServices;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database
{
    public interface IUnidadeDeTrabalhoNaoTransacional : IDisposable
    {
        IRepositorio<T> ObterRepositorio<T>() where T : class;

        Task<int> SalveAlteracoes();

    }
}
