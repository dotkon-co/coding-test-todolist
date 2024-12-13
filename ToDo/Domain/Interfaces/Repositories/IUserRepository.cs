using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
	public interface IUserRepository : IBaseRepository<UserEntity>
	{
		Task<IEnumerable<UserEntity>> GetAsync();
	}
}
