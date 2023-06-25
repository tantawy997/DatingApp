﻿using System.Security.Claims;

namespace DAtingApp.extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static string GetUserName(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name)?.Value;
		}
	}

}
