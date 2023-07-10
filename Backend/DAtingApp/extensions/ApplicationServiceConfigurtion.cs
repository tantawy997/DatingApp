using DatingApp.Data;
using DAtingApp.Data.repositories;
using DAtingApp.helpers;
using DAtingApp.interfaces;
using DAtingApp.interfaces.repositoryInterfaces;
using DAtingApp.Services;
using DAtingApp.SginalR;
using DAtingApp.UnitOfWorkRepo;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.extensions
{
    public static class ApplicationServiceConfigurtion
	{

		public static IServiceCollection AddApplicationService(this IServiceCollection services,
			IConfiguration configuration)
		{
			services.AddAuthorization();

			//services.AddDbContext<DataContext>(o =>
			//{
			//	o.UseNpgsql(configuration.GetConnectionString("co1"));

			//});
			services.AddScoped<ITokenService, TokenSerivce>();
			//services.AddScoped<IUserRepo, UserRepo>();
			services.Configure<cloudinarySettings>(configuration.GetSection("cloudinarySettings"));
			services.AddScoped<IPhotoService, PhotoService>();
			services.AddScoped<OnActionExcutionAsync>();
			//services.AddScoped<IUserLikeRepo, UserLikeRepo>();
			//services.AddScoped<IMessagesRepo, MessageRepo>();
			services.AddSignalR();
			services.AddSingleton<PresenceTracker>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
