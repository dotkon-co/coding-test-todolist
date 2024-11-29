using System.Runtime.CompilerServices;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database
{
    public interface IUnidadeDeTrabalho : IUnidadeDeTrabalhoNaoTransacional
    {
        Task InicieTransacao();

        Task FinalizeTransacao();

        Task RevertaTransacao();
    }

}
