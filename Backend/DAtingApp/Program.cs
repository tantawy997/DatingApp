using DatingApp.Data;
using DAtingApp.Data;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces;
using DAtingApp.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

							.AllowAnyOrigin()
							.AllowAnyMethod()
							.AllowAnyHeader();
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

			using var scope = app.Services.CreateScope();
			var serivce = scope.ServiceProvider;
			try
			{
				var context = serivce.GetRequiredService<DataContext>();
				await context.Database.MigrateAsync();

				await SeedDataContext.Seed(context);

			}
			catch (Exception ex)
			{
				var logger = serivce.GetRequiredService<ILogger<Program>>();

				logger.LogError(ex, "an error occured during seeding the data");
			}
			app.Run();
		}
	}
}