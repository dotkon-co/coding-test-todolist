namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Servicos
{
    public interface IServicoBase<T> where T : class
    {
        IQueryable<T> Query();
        Task AtualizeAsync(T objeto);
        Task AdicionarAsync(T objeto);
        Task AdicionarAsync(List<T> objetos);
    }
}
