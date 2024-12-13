using Domain.Entities;
using Domain.Requests.ToDo;

namespace Domain.Interfaces.Services
{
	public interface IToDoService : IBaseService
	{
		Task<TodoEntity> CreateAsync(ToDoCreateRequest todo);
		Task<TodoEntity?> GetAsync(Guid id);
		Task<IEnumerable<TodoEntity>> GetFromUserAsync(Guid userId);
	}
}
