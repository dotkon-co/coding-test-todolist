namespace UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database
{
    public interface IRepositorio<T> : IRepositorioDeLeitura<T> where T : class
    {

        /// <summary>
        /// Adiciona objeto na Base de Dados
        /// </summary>
        /// <param name="objeto">Objeto a ser adicionado</param>
        Task Adicionar(T objeto);

        /// <summary>
        /// Adiciona objetos na Base de Dados
        /// </summary>
        /// <param name="objeto">Objetos a serem adicionados</param>
        Task AdicionarTodos(List<T> objetos);

        /// <summary>
        /// Atualiza objeto na Base de Dados
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado</param>
        void Atualizar(T objeto);

        /// <summary>
        /// Remova objeto da base de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser removido</param>
        void Remover(T objeto);

        /// <summary>
        /// Remova objetos da base de dados
        /// </summary>
        /// <param name="objetos">Objetos a ser removido</param>
        void RemoverTodos(List<T> objetos);

    }
}