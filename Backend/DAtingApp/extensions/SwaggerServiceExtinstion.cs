using Microsoft.OpenApi.Models;

namespace DAtingApp.extensions
{
	public static class SwaggerServiceExtinstion
	{
		public static IServiceCollection AddSwaggerService(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api demo", Version = "v1" });
				var securityScheme = new OpenApiSecurityScheme
				{
					Description = "JWT auth bearer scheme",
					Name = "Autherization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Id = "Bearer",
						Type = ReferenceType.SecurityScheme
					}

				};

				c.AddSecurityDefinition("Bearer", securityScheme);

				var securityRequriment = new OpenApiSecurityRequirement()
				{
					{securityScheme, new []{ "bearer"} }
				};

				c.AddSecurityRequirement(securityRequriment);
			});
			return services;
		}
	}
}
