namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database
{
    public interface IUnidadeDeTrabalhoDeLeitura : IDisposable
    {
        IRepositorioDeLeitura<T> ObterRepositorio<T>() where T : class;
    }

}
