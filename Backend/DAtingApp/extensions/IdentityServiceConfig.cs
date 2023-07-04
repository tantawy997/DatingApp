using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DAtingApp.extensions
{
	public static class IdentityServiceConfig
	{
		public static IServiceCollection AddIdentityService(this IServiceCollection services,
			IConfiguration configuration)
		{

			services.AddIdentityCore<AppUser>(opt =>
			{
				opt.Password.RequireNonAlphanumeric = false;

			}).AddRoles<AppRole>()
			.AddRoleManager<RoleManager<AppRole>>()
			.AddEntityFrameworkStores<DataContext>();



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

			services.AddAuthorization(option =>
			{
				option.AddPolicy("RequireAnAdminRole", policy => policy.RequireRole("Admin"));
				option.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin","Moderator"));

			});
			return services;
		}
	}
}
