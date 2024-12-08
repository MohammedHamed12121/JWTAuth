using JWTAuth.Entites.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.API.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration _configuration) : DbContext(options)
	{
		public DbSet<ApplicationUser> ApplicationUser { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }

	}
}
