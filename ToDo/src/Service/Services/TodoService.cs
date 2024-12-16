using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.ToDo.Create;
using Domain.Responses.ToDo;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Service.Services
{
    public class TodoService : IToDoService
	{
		private readonly IToDoRepository _toDoRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _userId;

		public TodoService(IToDoRepository toDoRepository, IHttpContextAccessor httpContextAccessor)
        {
			_toDoRepository = toDoRepository;
			_httpContextAccessor = httpContextAccessor;
			_userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value!;

		}

		public async Task<bool> DeleteAsync(Guid id)
		{

			var todo = await _toDoRepository.GetAsync(id);
			if (todo == null || !todo.UserId.ToString().Equals(_userId))
				throw new DomainException("Task not found", 400);

			await _toDoRepository.DeleteAsync(todo);
			return true;
		}

		public async Task<TodoResponse?> GetAsync(Guid id)
		{
			var todo = await _toDoRepository.GetAsync(id);
			if (todo == null || !todo.UserId.ToString().Equals(_userId))
				throw new DomainException("Task not found", 400);
			return new TodoResponse(todo.Id, todo.Title, todo.Description, todo.CreatedAt, todo.FinishedAt, todo.UserId);
		}

		public async Task<IEnumerable<TodoResponse>> GetFromUserAsync()
		{
			var todos = await _toDoRepository.GetFromUserAsync(Guid.Parse(_userId));
			return todos.Select(todo => new TodoResponse(todo.Id, todo.Title, todo.Description, todo.CreatedAt, todo.FinishedAt, todo.UserId));
		}

		public async Task<TodoResponse> CreateAsync(ToDoCreateRequest todo)
		{
			var entity = await _toDoRepository.CreateAsync(new TodoEntity(todo.Title, todo.Description, Guid.Parse(_userId)));
			return new TodoResponse(entity.Id, entity.Title, entity.Description, entity.CreatedAt, entity.FinishedAt, entity.UserId);
		}
	}
}
