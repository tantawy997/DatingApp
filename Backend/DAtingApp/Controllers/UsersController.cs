using AutoMapper;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Controllers;
using DAtingApp.DTOs;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces;
using DAtingApp.interfaces.repositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingApp.Controller
{
	[Authorize]
	public class UsersController : ApiControllerBase
	{
		private readonly IUserRepo _UserRepo;
		private readonly IMapper _Mapper;
		private readonly IPhotoService _PhotoService;

		public UsersController(IUserRepo userRepo,IMapper mapper,IPhotoService photoService)
		{
			_UserRepo = userRepo;
			_Mapper = mapper;
			_PhotoService = photoService;
		}


		[HttpGet("GetUsers")]

		public async Task<ActionResult<PageList<MemberDTO>>> GetAllUsers([FromQuery] UserParams userParams)
		{

			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var user = await _UserRepo.GetUserByUserNameAsync(username);

			 userParams.CurrentUsername = user.UserName;
			if (string.IsNullOrEmpty(userParams.Gender))
			{
				//userParams.Gender = user.Gender == "male" ? "female" : "male";
				userParams.Gender = user.Gender == "male" ? "female" : "male";
			}
			var Users = await _UserRepo.GetMembersAsync(userParams);

			Response.AddPaginationHeader(new PaginationHeader(Users.CurrentPage, 
				Users.PageSize, Users.TotalCount, Users.TotalPages));

			return Ok(Users);
		}
		
		//[HttpGet("{id}")]
		//public async Task<ActionResult<MemberDTO>> GetUserById(Guid id)
		//{
		//	AppUser User = await _UserRepo.GetUserByIdAsync(id);
			

		//	if(User == null)
		//	{
		//		return NotFound();
		//	}
		//	var userdto = _Mapper.Map<AppUser, MemberDTO>(User);

		//	return Ok(userdto);

		//}

		[HttpGet("{UserName}")]
		public async Task<ActionResult<MemberDTO>> GetUserByUserName(string UserName)
		{
			if (UserName == null)
			{
				return BadRequest();
			}

			MemberDTO User = await _UserRepo.GetMemberAsync(UserName);

			if (User == null)
			{
				return NotFound();
			}
			//var userdto = _Mapper.Map<AppUser, MemberDTO>(User);

			return Ok(User);

		}
		[HttpPut]
		public async Task<ActionResult> UpdateUserAsync(UpdateUserDto updateUserDto)
		{
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var UserData = await _UserRepo.GetUserByUserNameAsync(username);

			if (UserData == null) return NotFound();

			_Mapper.Map(updateUserDto, UserData);

			if (await _UserRepo.SaveAllAsync())
			{
				return NoContent();
			}

			return BadRequest("you have failed to update the data");
		}

		[HttpPost("Add-Photo")]

		public async Task<ActionResult<PhotoDTO>> AddPhotoAsync(IFormFile file)
		{
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var UserData = await _UserRepo.GetUserByUserNameAsync(username);
			
			if(UserData == null) return NotFound();

			var result = await _PhotoService.AddPohtoAsync(file);

			if (result.Error != null) return BadRequest(result.Error.Message);

			var photo = new Photo
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId,

			};

			if (UserData.photos.Count == 0) photo.IsMain = true;

			UserData.photos.Add(photo);

			if (await _UserRepo.SaveAllAsync())
			{
				return CreatedAtAction(nameof(GetUserByUserName),
					new { UserName = username }, _Mapper.Map<PhotoDTO>(photo));
			}

			return BadRequest("problem uploading the image");
		}

		[HttpPut("set-main-photo/{PhotoId}")]

		public async Task<ActionResult> SetMainPhoto(Guid PhotoId)
		{
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var UserData = await _UserRepo.GetUserByUserNameAsync(username);

			if (UserData == null) return NotFound();

			var photo = UserData.photos.FirstOrDefault(user => user.PhotoId == PhotoId);
			
			if (photo == null) return NotFound();

			if (photo.IsMain) return BadRequest("this photo is already the main photo");

			var CurrentMain = UserData.photos.FirstOrDefault(x => x.IsMain);

			if (CurrentMain != null) CurrentMain.IsMain = false;

			photo.IsMain = true;

			if (await _UserRepo.SaveAllAsync()) return NoContent();


			return BadRequest("Problem setting the main photo");

		}
		[HttpDelete("delete-photo/{PhotoId}")]

		public async Task<ActionResult> DeletePhoto(Guid PhotoId)
		{
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var UserData = await _UserRepo.GetUserByUserNameAsync(username);

			var photo = UserData.photos.FirstOrDefault(x => x.PhotoId == PhotoId);

			if (photo == null) return NotFound();
			if (photo.IsMain) return BadRequest("You can not delete the main photo you " +
				"to update the main photo in order to delete this photo");

			if(photo.PublicId != null)
			{
				var result = await _PhotoService.DeletPhotoAsync(photo.PublicId);
				if (result.Error != null)
				{
					return BadRequest(result.Error.Message);
				}
			}
			UserData.photos.Remove(photo);

			if (await _UserRepo.SaveAllAsync()) return Ok();

			return BadRequest("problem deleting the photo");
		}
	}

}
