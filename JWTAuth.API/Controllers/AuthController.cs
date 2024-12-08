using JWTAuth.API.Data;
using JWTAuth.API.Services;
using JWTAuth.Entites.Models;
using JWTAuth.Entites.Requests;
using JWTAuth.Entites.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(AppDbContext _context, TokenProvider tokenProvider, RefreshTokenService refreshTokenService) : ControllerBase
	{
		[HttpPost("register")]
		public async Task<GeneralResponse> Register(RegisterRequest registerRequest)
		{
			var hashPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

			var user = await _context.ApplicationUser.FirstOrDefaultAsync(_ => _.Email == registerRequest.Email);
			if (user != null)
				return new GeneralResponse(false, "Email Already Exist");
			
			ApplicationUser User = new ApplicationUser
			{
				Email = registerRequest.Email,
				Password = hashPassword,
				Role = registerRequest.Role
			};

			await _context.ApplicationUser.AddAsync(User);
			await _context.SaveChangesAsync();
			return new GeneralResponse(true, "user registered..");
		}

		[HttpPost("login")]
		public async Task<AuthResponse> Login(LoginRequest loginRequest)
		{

			var user = await _context.ApplicationUser.FirstOrDefaultAsync(_ => _.Email == loginRequest.Email);
			if (user == null)
				return new AuthResponse(false, "Email Not Found");

			var hashedPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);

			if(!hashedPassword)
				return new AuthResponse(false, "Wrong Password");

			var token = tokenProvider.GenerateToken(user);

			await refreshTokenService.DisableUserTokenByEmail(loginRequest.Email);

			var isRefreshTokenInserted = await refreshTokenService.InsertRefreshToken(token.RefreshToken);
			if (!isRefreshTokenInserted)
				return new AuthResponse(false, "Can not insert the refresh token");


			return new AuthResponse(true, "", token.AccessToken, token.RefreshToken.Token);
		}

		[HttpPost("refresh-token")]
		public async Task<AuthResponse> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshtoken"];
			if (string.IsNullOrEmpty(refreshToken))
				return new AuthResponse(false, "Refresh Token Not Found In Cookies");

			var isValid = await refreshTokenService.IsRefreshTokenValid(refreshToken);
			if (!isValid) return new AuthResponse(false, "Token Not Valid");

			var currentUser = await tokenProvider.FindUserByToken(refreshToken);
			if (currentUser == null) return new AuthResponse(false, "User Not Found");

			var token = tokenProvider.GenerateToken(currentUser);

			var response = new AuthResponse(true, "", token.AccessToken, token.RefreshToken.Token);

			await refreshTokenService.DisableUserToken(refreshToken);
			await refreshTokenService.InsertRefreshToken(token.RefreshToken);
			return response;
		}


		[HttpPost("logout")]
		public async Task<AuthResponse> Logout()
		{
			var refreshToken = Request.Cookies["refreshtoken"];
			if (string.IsNullOrEmpty(refreshToken))
				return new AuthResponse(false, "Refresh Token Not Found In Cookies");

			var isValid = await refreshTokenService.IsRefreshTokenValid(refreshToken);
			if (!isValid) return new AuthResponse(false, "Token Not Valid");

			await refreshTokenService.DisableUserToken(refreshToken);

			return new AuthResponse(true, "Logout");
		}
	}
}
