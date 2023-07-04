using AutoMapper;
using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DAtingApp.Controllers
{
	//[Route("api/[controller]")]
	//[ApiController]
	public class AccountController : ApiControllerBase
	{
		//private readonly DataContext _Context;
		private readonly UserManager<AppUser> _UserManager;
		private readonly IMapper _Mapper;

		public ITokenService TokenService { get; }

		public AccountController(UserManager<AppUser> userManager,ITokenService tokenService,IMapper mapper)
		{
			//_Context = Context;
			_UserManager = userManager;
			TokenService = tokenService;
			_Mapper = mapper;
		}

		[HttpPost("register")]
		
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO userDto)
		{
			var checkexist = await  _UserManager.Users.AnyAsync(x => x.UserName == userDto.UserName.ToLower());

			if(checkexist)
			{
				return BadRequest("user name already taken");
			}
			var user = _Mapper.Map<AppUser>(userDto);


			user.UserName = userDto.UserName.ToLower();

			//var checkUser = await _UserManager.Users.FirstOrDefaultAsync(a => a.Id == user.Id);

			var result = await _UserManager.CreateAsync(user, userDto.Password);
			//await _Context.SaveChangesAsync();
			if (!result.Succeeded) return BadRequest(result.Errors);

			var userResult = await _UserManager.AddToRoleAsync(user,"Member");

			if (!userResult.Succeeded) return BadRequest(userResult.Errors);


			return Ok
			(
			new UserDTO 
			{
				Token = await TokenService.CreateToken(user) , 
				UserName = user.UserName,
				KnownAs = user.KnownAs,
				gender = user.Gender
			});
			
		}

		[HttpPost("login")]

		public async Task<ActionResult<UserDTO>> login(LoginDTO login)
		{
			AppUser user = await _UserManager.Users
				.Include(p => p.photos)
				.FirstOrDefaultAsync(x => x.UserName ==  login.UserName);
			if(user == null)
			{
				return Unauthorized("invalid username");
			}

			var result = await _UserManager.CheckPasswordAsync(user,login.Password);

			if (!result) return Unauthorized("invalid password");

			return Ok
			(
			new UserDTO
			{
				Token = await TokenService.CreateToken(user),
				UserName = user.UserName,
				PhotoUrl = user.photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				gender = user.Gender
			});
		}
	}
}
