using System.Security.Claims;
using System.Text;
using JWTAuth.API.Data;
using JWTAuth.Entites.Helpers;
using JWTAuth.Entites.Models;
using JWTAuth.Entites.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuth.API.Services
{
	public class TokenProvider(IConfiguration _config, AppDbContext _context)
	{
		public Token GenerateToken(ApplicationUser user)
		{
			var accessToken = GenerateAccessToken(user);
			var refreshToken = GenerateRefreshToken();
			refreshToken.Email = user.Email;
			return new Token
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		private string GenerateAccessToken(ApplicationUser user)
		{
			string secretKey = _config["JWT:SecretKey"];
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity([
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
					]),
				Expires = DateTime.Now.AddMinutes(1),
				SigningCredentials = credentials,
				Issuer = _config["JWT:Issuer"],
				Audience = _config["JWT:Audience"],
			};

			return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
		}

		public async Task<ApplicationUser?> FindUserByToken(string accessToken)
		{
			var token = await _context.RefreshTokens.Where(_ => _.Token == accessToken).AsNoTracking().FirstOrDefaultAsync();

			if (token == null)
				return null;

			var user = await _context.ApplicationUser.Where(_ => _.Email == token.Email).AsNoTracking().FirstOrDefaultAsync();

			return user;
		}
	
		private RefreshToken GenerateRefreshToken()
		{
			var refreshToken = new RefreshToken
			{
				Token = Guid.NewGuid().ToString(),
				Expires = DateTime.Now.AddMinutes(20),
				CreatedDate = DateTime.Now,
				Enabled = true
			};

			return refreshToken;
		}
	}

}
