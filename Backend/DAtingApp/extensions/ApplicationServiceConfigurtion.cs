using DatingApp.Data;
using DAtingApp.Data.repositories;
using DAtingApp.interfaces;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.extensions
{
	public static class ApplicationServiceConfigurtion
	{

		public static IServiceCollection AddApplicationService(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddAuthorization();
			services.AddDbContext<DataContext>(o =>
			{
				o.UseSqlServer(configuration.GetConnectionString("co1"));

			});
			services.AddScoped<ITokenService, TokenSerivce>();
			services.AddScoped<IUserRepo, UserRepo>();
			return services;
		}
	}
}
