using Domain.Interfaces.Services;
using Domain.Requests.User.Login;
using Domain.Requests.User.Register;
using Domain.Responses.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using System.IdentityModel.Tokens.Jwt;

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
			return await _userService.RegisterAsync(register);
			 
		}

		[HttpPost]
		[Route("Login")]
		public async Task<TokenResponse> LoginAsync([FromBody] LoginRequest login)
		{
			return await _userService.LoginAsync(login);
		}

		[Authorize]
		[HttpDelete]
		[Route("Delete")]
		public async Task<bool> UnregisterAsync()
		{
			return await _userService.UnregisterAsync();
		}
	}
}
