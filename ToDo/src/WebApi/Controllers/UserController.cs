using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		[HttpGet]
		public async Task<IEnumerable<UserResponse>> GetAsync()
		{
			return await _userService.GetAsync();
		}
	}
}
