using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.Data.repositories
{
	public class UserLikeRepo : IUserLikeRepo
	{
		private readonly DataContext _Context;

		public UserLikeRepo(DataContext context) 
		{
			_Context = context;
		}
		public async Task<UserLike> GetUserLikeAsync(Guid SourceLikeId, Guid TargetLikeId)
		{
			return await _Context.Likes.FindAsync(SourceLikeId, TargetLikeId);
		}

		public async Task<PageList<LikeDTO>> GetUserLikes(LikeParams likeParams)
		{
			var users = _Context.Users.Where(like => like.UserId == likeParams.UserId).AsQueryable();
			var likes = _Context.Likes.AsQueryable();
			
			if(likeParams.Predicate == "Liked")
			{
				likes = likes.Where(u => u.SourceUserId == likeParams.UserId);
				users = likes.Select(u => u.TargetUser);
			}
			if (likeParams.Predicate == "LikedBy")
			{
				likes = likes.Where(u => u.TargetUserId == likeParams.UserId);
				users = likes.Select(u => u.SourceUser);
			}

			var likedUsers = users.Select(user => new LikeDTO
			{
				Gender = user.Gender,
				KnownAs = user.KnownAs,
				photoUrl = user.photos.FirstOrDefault(p => p.IsMain).Url,
				UserName = user.UserName,
				Age = user.DateOfBirth.CalculateAge(),
				UserId = user.UserId,
				City = user.City
			});


			return	await PageList<LikeDTO>.CreateAsync(likedUsers, likeParams.PageNumber, likeParams.PageSize);
		}

		public async Task<AppUser> GetUsersWithLikes(Guid UserId)
		{
			return await _Context.Users.Include(u => u.LikedUsers)
				.FirstOrDefaultAsync(u => u.UserId == UserId);
		}
	}
}
