using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.helpers;

namespace DAtingApp.interfaces.repositoryInterfaces
{
	public interface IUserLikeRepo
	{
		Task<UserLike> GetUserLikeAsync(int SourceLikeId,int TargetLikeId);

		Task<AppUser> GetUsersWithLikes(int UserId);

		Task<PageList<LikeDTO>> GetUserLikes(LikeParams likeParams); 
	}
}
