using Domain.Entities;
using Domain.Requests.User.Login;
using Domain.Requests.User.Register;
using Domain.Responses.Register;
using Domain.Responses.User;

namespace Domain.Interfaces.Services
{
    public interface IUserService
	{
		Task<RegisterResponse> RegisterAsync(RegisterRequest user);
		Task<TokenResponse> LoginAsync(LoginRequest login);
		Task<IEnumerable<UserResponse>> GetAsync();
		Task<bool> UnregisterAsync();
	}
}
