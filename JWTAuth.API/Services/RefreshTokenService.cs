using JWTAuth.API.Data;
using JWTAuth.Entites.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.API.Services
{
	public class RefreshTokenService(AppDbContext _context)
	{
		public async Task<bool> InsertRefreshToken(RefreshToken token)
		{
			await _context.RefreshTokens.AddAsync(token);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DisableUserTokenByEmail(string email)
		{
			var token = await _context.RefreshTokens.Where(_ => _.Email == email).AsNoTracking().FirstOrDefaultAsync();
			if(token == null)
				return false;

			token.Enabled = false;

			_context.Entry(token).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DisableUserToken(string accessToken)
		{
			var token = await _context.RefreshTokens.Where(_ => _.Token == accessToken).AsNoTracking().FirstOrDefaultAsync();

			token.Enabled = false;

			_context.Entry(token).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> IsRefreshTokenValid(string accessToken)
		{
			var token = await _context.RefreshTokens.Where(_ => _.Token == accessToken && _.Enabled && _.Expires > DateTime.Now).AsNoTracking().FirstOrDefaultAsync();

			if (token == null)
				return false;

			return true;
		}
	}
}
