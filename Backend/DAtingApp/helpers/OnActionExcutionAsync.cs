using DAtingApp.extensions;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DAtingApp.helpers
{
	public class OnActionExcutionAsync : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var ExcutionResult = await next();

			if (!ExcutionResult.HttpContext.User.Identity.IsAuthenticated) return;

			var username = ExcutionResult.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			///User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var repo = ExcutionResult.HttpContext.RequestServices.GetRequiredService<IUserRepo>();

			var user = await repo.GetUserByUserNameAsync(username);

			user.LastActive = DateTime.UtcNow;

			await repo.SaveAllAsync();

		}
	}
}
