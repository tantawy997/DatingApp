using DatingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Add services to the container.
			builder.Services.AddAuthorization();
			builder.Services.AddDbContext<DataContext>(o =>
			{
				o.UseSqlServer(builder.Configuration.GetConnectionString("co1"));

			});

			builder.Services.AddControllers();

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

			app.UseAuthorization();

			
			app.MapControllers();

			app.Run();
		}
	}
}