using System.Text;
using JWTAuth.API.Data;
using JWTAuth.API.Extensions;
using JWTAuth.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlite(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.RequireHttpsMetadata = true;
		opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
			ValidAudience = builder.Configuration["JWT:Audience"],
			ValidIssuer = builder.Configuration["JWT:Issuer"],
			ClockSkew = TimeSpan.Zero
		};
	});
builder.Services.AddSwaggerAuth();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<RefreshTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
