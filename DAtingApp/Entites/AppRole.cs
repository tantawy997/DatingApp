using Microsoft.AspNetCore.Identity;

namespace DAtingApp.Entites
{
	public class AppRole : IdentityRole<int>
	{
		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}
