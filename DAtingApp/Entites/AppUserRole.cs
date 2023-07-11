using DatingApp.Entites;
using Microsoft.AspNetCore.Identity;

namespace DAtingApp.Entites
{
	public class AppUserRole : IdentityUserRole<Guid>
	{
		public AppUser  User { get; set; }

		public AppRole Role { get; set; }

	}
}
