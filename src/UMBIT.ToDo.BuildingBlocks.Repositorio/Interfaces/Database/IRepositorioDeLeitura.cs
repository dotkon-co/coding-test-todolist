namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database
{
    public interface IRepositorioDeLeitura<T> where T : class
    {
        /// <summary>
        /// Retorna uma query para realizar consultas
        /// </summary>
        /// <returns>Consulta sobre os objetos</returns>
        IQueryable<T> Query();


        /// <summary>
        /// Carrega objeto
        /// </summary>
        /// <param name="Objeto">Objeto a ser carregado</param>
        /// <returns>Objeto único</returns>
        T? Carregar(T Objeto);
    }
}
