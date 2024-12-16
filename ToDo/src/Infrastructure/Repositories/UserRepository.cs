using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDataContext _context;
		public UserRepository(AppDataContext context)
		{
			_context = context;
		}
		public async Task<UserEntity> CreateAsync(UserEntity user)
		{
			var entity = await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return entity.Entity;
		}

		public async Task DeleteAsync(UserEntity user)
		{
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}

		public async Task<IEnumerable<UserEntity>> GetAsync() 
			=> await _context.Users.AsNoTracking().ToListAsync();
		

		public async Task<UserEntity?> GetAsync(Guid id)
			=> await _context.Users.FindAsync(id);

		public async Task<UserEntity?> GetAsync(string user)
			=> await _context.Users.FirstOrDefaultAsync(x => x.User == user);
	}
}
