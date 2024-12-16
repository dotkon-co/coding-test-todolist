using Domain.Entities;
using Domain.Requests.ToDo;
using Domain.Responses.ToDo;

namespace Domain.Interfaces.Services
{
	public interface IToDoService : IBaseService
	{
		Task<TodoResponse> CreateAsync(ToDoCreateRequest todo);
		Task<TodoResponse?> GetAsync(Guid id);
		Task<IEnumerable<TodoResponse>> GetFromUserAsync();
	}
}
