using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Data;
using DAtingApp.Entites;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces;
using DAtingApp.Middleware;
using DAtingApp.SginalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DAtingApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Add services to the container.

			builder.Services.AddApplicationService(builder.Configuration);
			builder.Services.AddControllers();
			
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			
			builder.Services.AddIdentityService(builder.Configuration);
			builder.Services.AddSwaggerService();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					policyBuilder =>
					{
						policyBuilder


							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials()
							.WithOrigins("https://localhost:4200");
					});
			});
			builder.Services.AddHttpContextAccessor();

			var app = builder.Build();
			app.UseMiddleware<ExceptionMiddleware>();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseCors("AllowAll");

			app.UseStaticFiles();

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			
			app.MapControllers();

			app.MapHub<PresenseHub>("hubs/presence");
			app.MapHub<MessageHub>("hubs/message");


			using var scope = app.Services.CreateScope();
			var service = scope.ServiceProvider;
			try
			{
				var context = service.GetRequiredService<DataContext>();
				var userManager = service.GetRequiredService<UserManager<AppUser>>();
				var roleManager = service.GetRequiredService<RoleManager<AppRole>>();

				await context.Database.MigrateAsync();
				await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
				await SeedDataContext.Seed(userManager,context,roleManager);

			}
			catch (Exception ex)
			{
				var logger = service.GetRequiredService<ILogger<Program>>();

				logger.LogError(ex, "an error occured during seeding the data");
			}
			app.Run();
		}
	}
}