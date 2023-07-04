﻿using Microsoft.AspNetCore.Identity;

namespace DAtingApp.Entites
{
	public class AppRole : IdentityRole<Guid>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
