using DatingApp.Data;
using DAtingApp.extensions;
using DAtingApp.interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DAtingApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Add services to the container.

			builder.Services.AddApplicationService(builder.Configuration);
			builder.Services.AddControllers();

			builder.Services.AddIdentityService(builder.Configuration);
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

			app.Run();
		}
	}
}