using AutoMapper;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Controllers;
using DAtingApp.DTOs;
using DAtingApp.extensions;
using DAtingApp.helpers;
using DAtingApp.interfaces;
using DAtingApp.interfaces.repositoryInterfaces;
using DAtingApp.UnitOfWorkRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingApp.Controller
{
	[Authorize]
	public class UsersController : ApiControllerBase
	{
		private readonly IUnitOfWork _UnitOfWork;
		private readonly IMapper _Mapper;
		private readonly IPhotoService _PhotoService;

		public UsersController(IUnitOfWork unitOfWork,IMapper mapper,IPhotoService photoService)
		{
			_UnitOfWork = unitOfWork;
			_Mapper = mapper;
			_PhotoService = photoService;
		}


		[HttpGet("GetUsers")]

		public async Task<ActionResult<PageList<MemberDTO>>> GetAllUsers([FromQuery] UserParams userParams)
		{

			//var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			var gender = await _UnitOfWork._UserRepo.GetUserGender(User.GetUserName());

			 userParams.CurrentUsername = User.GetUserName();

			if (string.IsNullOrEmpty(userParams.Gender))
			{
				//userParams.Gender = user.Gender == "male" ? "female" : "male";
				userParams.Gender = gender == "male" ? "female" : "male";
			}
			var Users = await _UnitOfWork._UserRepo.GetMembersAsync(userParams);

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

			MemberDTO User = await _UnitOfWork._UserRepo.GetMemberAsync(UserName);

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
			var username = User.GetUserName();

			var UserData = await _UnitOfWork._UserRepo.GetUserByUserNameAsync(username);

			if (UserData == null) return NotFound();

			_Mapper.Map(updateUserDto, UserData);

			if (await _UnitOfWork.Completes())
			{
				return NoContent();
			}

			return BadRequest("you have failed to update the data");
		}

		[HttpPost("Add-Photo")]

		public async Task<ActionResult<PhotoDTO>> AddPhotoAsync(IFormFile file)
		{
			var username = User.GetUserName();

			var UserData = await _UnitOfWork._UserRepo.GetUserByUserNameAsync(username);
			
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

			if (await _UnitOfWork.Completes())
			{
				return CreatedAtAction(nameof(GetUserByUserName),
					new { UserName = username }, _Mapper.Map<PhotoDTO>(photo));
			}

			return BadRequest("problem uploading the image");
		}

		[HttpPut("set-main-photo/{PhotoId}")]

		public async Task<ActionResult> SetMainPhoto(int PhotoId)
		{
			var username = User.GetUserName();

			var UserData = await _UnitOfWork._UserRepo.GetUserByUserNameAsync(username);

			if (UserData == null) return NotFound();

			var photo = UserData.photos.FirstOrDefault(user => user.PhotoId == PhotoId);
			
			if (photo == null) return NotFound();

			if (photo.IsMain) return BadRequest("this photo is already the main photo");

			var CurrentMain = UserData.photos.FirstOrDefault(x => x.IsMain);

			if (CurrentMain != null) CurrentMain.IsMain = false;

			photo.IsMain = true;

			if (await _UnitOfWork.Completes()) return NoContent();


			return BadRequest("Problem setting the main photo");

		}
		[HttpDelete("delete-photo/{PhotoId}")]

		public async Task<ActionResult> DeletePhoto(int PhotoId)
		{
			var username = User.GetUserName();

			var UserData = await _UnitOfWork._UserRepo.GetUserByUserNameAsync(username);

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

			if (await _UnitOfWork.Completes()) return Ok();

			return BadRequest("problem deleting the photo");
		}
	}

}
