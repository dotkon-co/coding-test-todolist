using Domain.Entities;
using Domain.Requests.ToDo.Create;
using Domain.Responses.ToDo;

namespace Domain.Interfaces.Services
{
    public interface IToDoService
	{
		Task<TodoResponse> CreateAsync(ToDoCreateRequest todo);
		Task<TodoResponse?> GetAsync(Guid id);
		Task<IEnumerable<TodoResponse>> GetFromUserAsync();
		Task<bool> DeleteAsync(Guid id);
		Task<TodoResponse> CompleteAsync(Guid id);
	}
}
