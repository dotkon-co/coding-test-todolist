using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.ToDo;
using Domain.Responses.ToDo;

namespace Service.Services
{
	public class TodoService : IToDoService
	{
		private readonly IToDoRepository _toDoRepository;
        public TodoService(IToDoRepository toDoRepository)
        {
			_toDoRepository = toDoRepository;
        }

		public async Task<bool> DeleteAsync(Guid id)
		{
			var todo = await _toDoRepository.GetAsync(id);
			if (todo == null)
				return false;

			await _toDoRepository.DeleteAsync(todo);
			return true;
		}

		public async Task<TodoResponse?> GetAsync(Guid id)
		{
			var todo = await _toDoRepository.GetAsync(id);
			if (todo == null) 
				return null;
			return new TodoResponse(todo.Id, todo.Title, todo.Description, todo.CreatedAt, todo.FinishedAt, todo.UserId);
		}

		public async Task<IEnumerable<TodoResponse>> GetFromUserAsync(Guid userId)
		{
			var todos = await _toDoRepository.GetFromUserAsync(userId);
			return todos.Select(todo => new TodoResponse(todo.Id, todo.Title, todo.Description, todo.CreatedAt, todo.FinishedAt, todo.UserId));
		}

		public async Task<TodoResponse> CreateAsync(ToDoCreateRequest todo)
		{
			var entity = new TodoEntity(todo.Title, todo.Description, Guid.Parse("aed86e79-f212-4029-8735-d68a772bb617"));
			return new TodoResponse(entity.Id, entity.Title, entity.Description, entity.CreatedAt, entity.FinishedAt, entity.UserId);
		}
	}
}
