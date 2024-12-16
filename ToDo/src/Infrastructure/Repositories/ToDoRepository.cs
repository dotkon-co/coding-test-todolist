using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ToDoRepository : IToDoRepository
	{
		private readonly AppDataContext _context;
		public ToDoRepository(AppDataContext context)
		{
			_context = context;
		}

		public async Task<TodoEntity> CreateAsync(TodoEntity todo)
		{
			var entity = await _context.ToDos.AddAsync(todo);
			await _context.SaveChangesAsync();
			return entity.Entity;
		}

		public async Task DeleteAsync(TodoEntity entity)
		{
			_context.ToDos.Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<TodoEntity?> GetAsync(Guid id) 
			=> await _context.ToDos.FindAsync(id);

		public async Task<IEnumerable<TodoEntity>> GetFromUserAsync(Guid userId) 
			=> await _context.ToDos.Where(p => p.UserId == userId).ToListAsync();

		public async Task<TodoEntity> UpdateAsync(TodoEntity todo)
		{
			var entity = _context.ToDos.Update(todo);
			await _context.SaveChangesAsync();
			return entity.Entity;
		}
	}
}
