using Domain.Entities;
using Domain.Responses.Register;
using Domain.Responses.User;

namespace Domain.Interfaces.Services
{
	public interface ITokenService
	{
		TokenResponse GenerateToken(UserEntity user);
	}
}
