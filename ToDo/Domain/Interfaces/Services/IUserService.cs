using Domain.Entities;
using Domain.Requests.Register;
using Domain.Responses.User;

namespace Domain.Interfaces.Services
{
	public interface IUserService : IBaseService
	{
		Task<UserEntity> RegisterAsync(RegisterRequest user);
		Task<IEnumerable<UserResponse>> GetAsync();
	}
}
