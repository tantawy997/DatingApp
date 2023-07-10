using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.helpers;

namespace DAtingApp.interfaces.repositoryInterfaces
{
	public interface IUserRepo
	{
		void Update(AppUser user);

		Task<IEnumerable<AppUser>> GetUsersAsync();

		Task<AppUser> GetUserByIdAsync(Guid id);

		Task<AppUser> GetUserByUserNameAsync(string UserName);

		//Task<bool> SaveAllAsync();

		Task<PageList<MemberDTO>> GetMembersAsync(UserParams userParams);

		Task<MemberDTO> GetMemberAsync(string UserName);

		Task<string> GetUserGender(string username);
	}
}
