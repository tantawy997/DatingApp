using DAtingApp.extensions;
using DAtingApp.interfaces.repositoryInterfaces;
using DAtingApp.UnitOfWorkRepo;
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
			var unitOfWork = ExcutionResult.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

			var user = await unitOfWork._UserRepo.GetUserByUserNameAsync(username);

			user.LastActive = DateTime.UtcNow;

			await unitOfWork.Completes();

		}
	}
}
