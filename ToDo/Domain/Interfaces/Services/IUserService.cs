using Domain.Entities;
using Domain.Requests.Register;

namespace Domain.Interfaces.Services
{
	public interface IUserService : IBaseService
	{
		Task<UserEntity> RegisterAsync(RegisterRequest user);
		Task<IEnumerable<UserEntity>> GetAsync();
	}
}
