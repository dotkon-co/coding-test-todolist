using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.Register;
using Domain.Responses.User;

namespace Service.Services
{
	public class UserService : IUserService
	{
		private readonly IEncryptService _encryptService;
		private readonly IUserRepository _userRepository;
		public UserService(IEncryptService encryptService, IUserRepository userRepository)
		{
			_encryptService = encryptService;
			_userRepository = userRepository;
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

		public async Task<UserEntity> RegisterAsync(RegisterRequest user)
		{
			user.Password = _encryptService.EncryptString(user.Password);
			var entity = new UserEntity(user.Name, user.User, user.Password);
			return await _userRepository.CreateAsync(entity);
		}
	}
}
