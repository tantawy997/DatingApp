using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.helpers;

namespace DAtingApp.interfaces.repositoryInterfaces
{
	public interface IUserLikeRepo
	{
		Task<UserLike> GetUserLikeAsync(Guid SourceLikeId,Guid TargetLikeId);

		Task<AppUser> GetUsersWithLikes(Guid UserId);

		Task<PageList<LikeDTO>> GetUserLikes(LikeParams likeParams); 
	}
}
