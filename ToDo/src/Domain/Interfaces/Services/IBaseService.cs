using Domain.Entities;

namespace Domain.Interfaces.Services
{
	public interface IBaseService
	{
		Task<bool> DeleteAsync(Guid id);
	}
}
