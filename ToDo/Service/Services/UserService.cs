using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.User;
using Domain.Responses.User;

namespace Service.Services
{
	public class UserService : IUserService
	{
		private readonly IEncryptService _encryptService;
		private readonly IUserRepository _userRepository;
		private readonly ITokenService _tokenService;
		public UserService(IEncryptService encryptService, IUserRepository userRepository, ITokenService tokenService)
		{
			_encryptService = encryptService;
			_userRepository = userRepository;
			_tokenService = tokenService;
		}
		public async Task<bool> DeleteAsync(Guid id)
		{
			var user = await _userRepository.GetAsync(id);
			if (user == null)
				return false;

			await _userRepository.DeleteAsync(user);
			return true;
		}

		public async Task<IEnumerable<UserResponse>> GetAsync()
		{
			var users = await _userRepository.GetAsync();
			return users.Select(x => new UserResponse(x.Id ,x.Name, x.User, x.CreatedAt));
		}

		public async Task<UserEntity> RegisterAsync(RegisterRequest register)
		{
			register.Password = _encryptService.HashString(register.Password);
			var entity = new UserEntity(register.Name, register.User, register.Password);
			return await _userRepository.CreateAsync(entity);
		}

		public async Task<string> LoginAsync(LoginRequest login)
		{
			var user = await _userRepository.GetAsync(login.User);
			var validPassword = _encryptService.CheckHash(login.Password, user.Password ?? "");
			if (user == null || !validPassword)
				return "";

			return _tokenService.GenerateToken(user);
		}
	}
}
