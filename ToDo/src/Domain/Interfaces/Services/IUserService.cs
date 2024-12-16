using Domain.Entities;
using Domain.Requests.User;
using Domain.Responses.Register;
using Domain.Responses.User;

namespace Domain.Interfaces.Services
{
	public interface IUserService : IBaseService
	{
		Task<UserEntity> RegisterAsync(RegisterRequest user);
		Task<TokenResponse> LoginAsync(LoginRequest login);
		Task<IEnumerable<UserResponse>> GetAsync();
	}
}
