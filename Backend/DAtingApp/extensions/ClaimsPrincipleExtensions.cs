using System.Security.Claims;

namespace DAtingApp.extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static string GetUserName(this ClaimsPrincipal User)
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public static string GetUserId(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name)?.Value;
		}
	}

}
