using AutoMapper;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Controllers;
using DAtingApp.DTOs;
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

		public UsersController(IUserRepo userRepo,IMapper mapper)
		{
			_UserRepo = userRepo;
			_Mapper = mapper;
		}


		[HttpGet("GetUsers")]

		public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAllUsers()
		{
			var Users = await _UserRepo.GetMembersAsync();
			//var usersDTO = _Mapper.Map<IEnumerable<MemberDTO>>(Users);
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
	}

}
