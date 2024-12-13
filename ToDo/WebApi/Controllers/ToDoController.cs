using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Requests.ToDo;
using Domain.Responses.ToDo;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
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
			var id = Guid.Parse("aed86e79-f212-4029-8735-d68a772bb617");
			return await _todoService.GetFromUserAsync(id);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<bool> DeleteAsync(Guid id)
		{
			return await _todoService.DeleteAsync(id);
		}
	}
}
