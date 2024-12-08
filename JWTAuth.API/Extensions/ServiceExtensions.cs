using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace JWTAuth.API.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
		{
			services.AddSwaggerGen(opt =>
			{
				var securityDef = new OpenApiSecurityScheme
				{
					Name = "Authentication",
					Type = SecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = "Jwt",
					In = ParameterLocation.Header,
					Description = "JWT Authentication header using bearer scheme."
				};

				opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDef);

				opt.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = JwtBearerDefaults.AuthenticationScheme
							}
						},
						new string[] { }
					}
				});
			});

			return services;
		}
	}
}
