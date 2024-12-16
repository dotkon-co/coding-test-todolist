using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Requests.ToDo;
using Domain.Responses.ToDo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class ToDoController : ControllerBase
	{
		private readonly IToDoService _todoService;
		public ToDoController(IToDoService todoService)
		{
			_todoService = todoService;
		}

		[HttpPost]
		public async Task<TodoResponse> CreateAsync([FromBody] ToDoCreateRequest todo)
		{
			return await _todoService.CreateAsync(todo);
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<TodoResponse?> GetAsync(Guid id)
		{
			return await _todoService.GetAsync(id);
		}

		[HttpGet]
		public async Task<IEnumerable<TodoResponse>> GetAsync()
		{
			return await _todoService.GetFromUserAsync();
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<bool> DeleteAsync(Guid id)
		{
			return await _todoService.DeleteAsync(id);
		}
	}
}
