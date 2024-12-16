using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
	public interface IToDoRepository : IBaseRepository<TodoEntity>
	{
		Task<IEnumerable<TodoEntity>> GetFromUserAsync(Guid userId);
		Task<TodoEntity> UpdateAsync(TodoEntity todo);
	}
}
