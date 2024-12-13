using Domain.Interfaces.Services;
using Domain.Requests.User;
using Domain.Responses.Register;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
		[Route("Register")]
		public async Task<RegisterResponse> RegisterAsync([FromBody] RegisterRequest register)
		{
			var response = await _userService.RegisterAsync(register);
			return new RegisterResponse(response.Name, response.User);
		}

		[HttpPost]
		[Route("Login")]
		public async Task<RegisterResponse> LoginAsync([FromBody] LoginRequest login)
		{
			var response = await _userService.LoginAsync(login);
			return null;
			//return new RegisterResponse(response.Name, response.User);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<bool> UnregisterAsync()
		{
			var id = Guid.Parse("aed86e79-f212-4029-8735-d68a772bb617");
			return await _userService.DeleteAsync(id);
		}
	}
}
