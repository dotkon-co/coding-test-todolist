﻿using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests.User;
using Domain.Responses.Register;
using Domain.Responses.User;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Service.Services
{
	public class UserService : IUserService
	{
		private readonly IEncryptService _encryptService;
		private readonly IUserRepository _userRepository;
		private readonly ITokenService _tokenService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public UserService(IEncryptService encryptService, IUserRepository userRepository, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
		{
			_encryptService = encryptService;
			_userRepository = userRepository;
			_tokenService = tokenService;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<bool> DeleteAsync(Guid id)
		{
			var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value!;
			var user = await _userRepository.GetAsync(id);
			if (user == null || !userId.Equals(user.Id.ToString()))
				throw new DomainException("User not found", 400);

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

		public async Task<TokenResponse> LoginAsync(LoginRequest login)
		{
			var user = await _userRepository.GetAsync(login.User);
			var validPassword = _encryptService.CheckHash(login.Password, user?.Password ?? "");
			if (user == null || !validPassword)
				throw new DomainException("User not found", 400);

			return new TokenResponse(_tokenService.GenerateToken(user));
		}
	}
}