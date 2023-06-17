using DatingApp.Entites;

namespace DAtingApp.interfaces
{
	public interface ITokenService
	{
		string CreateToken(AppUser user);
	}
}
