using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.DTOs;
using DAtingApp.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DAtingApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ApiControllerBase
	{
		private readonly DataContext _Context;

		public ITokenService TokenService { get; }

		public AccountController(DataContext Context,ITokenService tokenService)
		{
			_Context = Context;
			TokenService = tokenService;
		}

		[HttpPost("register")]
		
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO userDTO)
		{
			var checkexist = await  _Context.Users.AnyAsync(x => x.UserName == userDTO.UserName.ToLower());
			if(checkexist)
			{
				return BadRequest("user name already taken");
			}
			using var macHash = new HMACSHA512();

			var user = new AppUser
			{
				UserName = userDTO.UserName.ToLower(),
				PasswordHah =  macHash.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
				PasswordSalt = macHash.Key
			};
			var checkUser = await _Context.Users.FirstOrDefaultAsync(a => a.id == user.id);

			await _Context.AddAsync(user);
			await _Context.SaveChangesAsync();
			return Ok
			(
			new UserDTO 
			{

			Token = TokenService.CreateToken(user) , 
			UserName = user.UserName

			});
			
		}

		[HttpPost("login")]

		public async Task<ActionResult<UserDTO>> login(LoginDTO login)
		{
			AppUser user = await _Context.Users.FirstOrDefaultAsync(x => x.UserName ==  login.UserName);
			if(user == null)
			{
				return Unauthorized("invalid username");
			}

			var HashedPass = new HMACSHA512(user.PasswordSalt);
			var ComputedHAshed = HashedPass.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

			for (int i = 0; i < ComputedHAshed.Length; i++)
			{
				if (ComputedHAshed[i] != user.PasswordHah[i]) return Unauthorized("invalid password");
			}

			return Ok
			(
			new UserDTO
			{

			Token = TokenService.CreateToken(user),
			UserName = user.UserName

			});
		}
	}
}
