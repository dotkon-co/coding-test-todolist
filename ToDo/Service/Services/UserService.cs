using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.Register;

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

		public async Task<IEnumerable<UserEntity>> GetAsync()
		{
			return await _userRepository.GetAsync();
		}

		public async Task<UserEntity> RegisterAsync(RegisterRequest user)
		{
			user.Password = _encryptService.EncryptString(user.Password);
			var entity = new UserEntity(user.Name, user.User, user.Password);
			return await _userRepository.CreateAsync(entity);
		}
	}
}
