using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.ToDo;

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

		public async Task<TodoEntity?> GetAsync(Guid id)
		{
			return await _toDoRepository.GetAsync(id);
		}

		public async Task<IEnumerable<TodoEntity>> GetFromUserAsync(Guid userId)
		{
			return await _toDoRepository.GetFromUserAsync(userId);
		}

		public async Task<TodoEntity> CreateAsync(ToDoCreateRequest todo)
		{
			var entity = new TodoEntity(todo.Title, todo.Description);
			return await _toDoRepository.CreateAsync(entity);
		}
	}
}
