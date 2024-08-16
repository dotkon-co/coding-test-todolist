using Core.Todo.Entities;

namespace Core.Todo.Repositories
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(long id);
        Task<TEntity> GetById(long id);
        Task<List<TEntity>> GetAll();
    }
}
