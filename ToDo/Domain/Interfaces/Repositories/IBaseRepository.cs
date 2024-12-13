using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
	public interface IBaseRepository<Entity> where Entity : BaseEntity
	{
		Task<Entity> CreateAsync(Entity entity);
		Task DeleteAsync(Entity entity);
	}
}
