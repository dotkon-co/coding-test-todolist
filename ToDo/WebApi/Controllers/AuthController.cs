using Domain.Interfaces.Services;
using Domain.Requests.Register;
using Domain.Responses.Register;
using Microsoft.AspNetCore.Mvc;

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
		public async Task<RegisterResponse> Register([FromBody] RegisterRequest register)
		{
			var response = await _userService.RegisterAsync(register);
			return new RegisterResponse(response.Name, response.User);
		}
	}
}
