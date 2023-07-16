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

			builder.Services.AddControllers();
			builder.Services.AddApplicationService(builder.Configuration);
			
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			
			builder.Services.AddIdentityService(builder.Configuration);

			var connString = "";
			if (builder.Environment.IsDevelopment())
				connString = builder.Configuration.GetConnectionString("co1");
			else
			{
				// Use connection string provided at runtime by FlyIO.
				var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

				// Parse connection URL to connection string for Npgsql
				connUrl = connUrl.Replace("postgres://", string.Empty);
				var pgUserPass = connUrl.Split("@")[0];
				var pgHostPortDb = connUrl.Split("@")[1];
				var pgHostPort = pgHostPortDb.Split("/")[0];
				var pgDb = pgHostPortDb.Split("/")[1];
				var pgUser = pgUserPass.Split(":")[0];
				var pgPass = pgUserPass.Split(":")[1];
				var pgHost = pgHostPort.Split(":")[0];
				var pgPort = pgHostPort.Split(":")[1];

				connString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};sslmode=allow;";

			}
			builder.Services.AddDbContext<DataContext>(opt =>
			{
				//opt.EnableSensitiveDataLogging();
				//var connString = builder.Configuration.GetConnectionString("co1");
				opt.UseNpgsql(connString);
			});


			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			//builder.Services.AddEndpointsApiExplorer();
			//builder.Services.AddSwaggerGen();
			//builder.Services.AddSwaggerService();

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					policyBuilder =>
					{
						policyBuilder


							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials()
							//.AllowAnyOrigin();
							.WithOrigins("https://localhost:4200/");
					});
			});
			//builder.Services.AddHttpContextAccessor();
			
			var app = builder.Build();
			app.UseMiddleware<ExceptionMiddleware>();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
				app.UseHttpsRedirection();
			}

			app.UseCors("AllowAll");

			AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


			app.UseAuthentication();
			app.UseAuthorization();
			app.UseDefaultFiles();

			app.UseStaticFiles();
			

			app.MapControllers();

			app.MapHub<PresenseHub>("hubs/presence");
			app.MapHub<MessageHub>("hubs/message");

			app.MapFallbackToController("Index", "FallBack");
			//app.UseHttpsRedirection();

			using var scope = app.Services.CreateScope();
			var service = scope.ServiceProvider;
			try
			{
				var context = service.GetRequiredService<DataContext>();
				var userManager = service.GetRequiredService<UserManager<AppUser>>();
				var roleManager = service.GetRequiredService<RoleManager<AppRole>>();

				await context.Database.MigrateAsync();

				await SeedDataContext.ClearConnections(context);

				await SeedDataContext.Seed(userManager,context,roleManager);

			}
			catch (Exception ex)
			{
				var logger = service.GetRequiredService<ILogger<Program>>();

				logger.LogError(ex.Message, "an error occured during seeding the data");
				logger.LogError(ex.Source, ex.InnerException);
			}
			app.Run();
		}
	}
}