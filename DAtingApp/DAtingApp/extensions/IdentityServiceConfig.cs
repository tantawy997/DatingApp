using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DAtingApp.extensions
{
	public static class IdentityServiceConfig
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(option =>
			{
			option.TokenValidationParameters = new TokenValidationParameters
			{
			ValidateIssuerSigningKey = true,
			ValidateAudience = false,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(configuration["TokenKey"])),
			ValidateIssuer = false
			};
			
			});

			return services;
		}
	}
}
