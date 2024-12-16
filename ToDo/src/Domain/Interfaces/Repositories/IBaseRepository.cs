using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
	public interface IBaseRepository<Entity> where Entity : BaseEntity
	{
		Task<Entity?> GetAsync(Guid id);
		Task<Entity> CreateAsync(Entity entity);
		Task DeleteAsync(Entity entity);
	}
}
