using Domain.Interfaces.Services;
using Domain.Requests.Register;
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
		public async Task<RegisterResponse> RegisterAsync([FromBody] RegisterRequest register)
		{
			var response = await _userService.RegisterAsync(register);
			return new RegisterResponse(response.Name, response.User);
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
