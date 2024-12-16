using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Responses.Register;
using Domain.Responses.User;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
	public class TokenService : ITokenService
	{
		private readonly JwtSettings _jwtSettings;
		public TokenService(IOptions<JwtSettings> jwtSettings)
		{
			_jwtSettings = jwtSettings.Value;
		}
		public TokenResponse GenerateToken(UserResponse user)
		{
			var claims = new List<Claim> {
				new Claim(JwtRegisteredClaimNames.Sub, user.Name),
				new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: claims, signingCredentials: creds);
			return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
		}
	}
}
