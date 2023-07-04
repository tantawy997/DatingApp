using DatingApp.Entites;

namespace DAtingApp.interfaces
{
	public interface ITokenService
	{
		Task<string> CreateToken(AppUser user);
	}
}
