using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Requests.Register;

namespace Service.Services
{
	public class TodoService : IUserService
	{
		public Task<bool> DeleteAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<UserEntity>> GetAsync()
		{
			throw new NotImplementedException();
		}

		public Task<UserEntity> RegisterAsync(RegisterRequest user)
		{
			throw new NotImplementedException();
		}
	}
}
