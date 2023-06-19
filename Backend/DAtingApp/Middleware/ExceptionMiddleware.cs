using DAtingApp.errors;
using System.Net;
using System.Text.Json;

namespace DAtingApp.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _Next;
		private readonly ILogger<ExceptionMiddleware> _Logger;
		private readonly IHostEnvironment _HostEnvironment;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
			IHostEnvironment hostEnvironment)
		{
			_Next = next;
			_Logger = logger;
			_HostEnvironment = hostEnvironment;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{

				await _Next(context);
			}
			catch (Exception ex)
			{

				_Logger.LogError(ex,ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				var response = (_HostEnvironment.IsDevelopment())
					? new ApiError(context.Response.StatusCode, ex.Message,
					ex.StackTrace?.ToString()) :
					new ApiError(context.Response.StatusCode, ex.Message,
					"internal server error");

				var options = new JsonSerializerOptions
				{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};
				var json = JsonSerializer.Serialize(response, options);

				await context.Response.WriteAsync(json);
			
			}
		}
	}
}
