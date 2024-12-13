using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
	public interface IToDoRepository : IBaseRepository<TodoEntity>
	{
		Task<TodoEntity?> GetAsync(Guid id);
		Task<IEnumerable<TodoEntity>> GetFromUserAsync(Guid userId);
		Task<TodoEntity> UpdateAsync(TodoEntity todo);
	}
}
