using DAtingApp.DTOs;
using DAtingApp.Entites;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces.repositoryInterfaces;
using DAtingApp.UnitOfWorkRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace DAtingApp.Controllers
{
	//[Route("api/[controller]")]
	//[ApiController]
	public class LikeController : ApiControllerBase
	{
		private readonly IUnitOfWork _UnitOfWork;

		public LikeController(IUnitOfWork unitOfWork) 
		{
			_UnitOfWork = unitOfWork;
		}

		[HttpPost("{UserName}")]
		public async Task<ActionResult<string>> AddLike(string UserName)
		{
			var SourceUserId =  new Guid(User.GetUserId());

			var LikedUser = await _UnitOfWork._UserRepo.GetUserByUserNameAsync(UserName);

			var SourceUser = await _UnitOfWork._UserLikeRepo.GetUsersWithLikes(SourceUserId);

			if (LikedUser == null) return NotFound();
			if (SourceUser.UserName == UserName) return BadRequest("you can not like your self");

			var userLike = await _UnitOfWork._UserLikeRepo.GetUserLikeAsync(SourceUserId, LikedUser.Id);

			if (userLike != null) return BadRequest("you already liked this user");

			userLike = new UserLike
			{
				SourceUserId = SourceUserId,
				TargetUserId = LikedUser.Id
			};

			
			SourceUser.LikedUsers.Add(userLike);

			if (await _UnitOfWork.Completes()) return Ok();

			return BadRequest("failed to like user");
			
		}

		[HttpGet]

		public async Task<ActionResult<PageList<LikeDTO>>> GetUserLikes([FromQuery] LikeParams likeParams)
		{
			likeParams.UserId = new Guid(User.GetUserId());
			var user = await _UnitOfWork._UserLikeRepo.GetUserLikes(likeParams);
			Response.AddPaginationHeader(new PaginationHeader(user.CurrentPage,user.PageSize,
				user.TotalCount, user.TotalPages));

			return Ok(user);
		}
	}


}
