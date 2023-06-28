using AutoMapper;
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
		private readonly IMapper _Mapper;

		public ITokenService TokenService { get; }

		public AccountController(DataContext Context,ITokenService tokenService,IMapper mapper)
		{
			_Context = Context;
			TokenService = tokenService;
			_Mapper = mapper;
		}

		[HttpPost("register")]
		
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO userDto)
		{
			var checkexist = await  _Context.Users.AnyAsync(x => x.UserName == userDto.UserName.ToLower());

			if(checkexist)
			{
				return BadRequest("user name already taken");
			}
			var user = _Mapper.Map<AppUser>(userDto);

			using var macHash = new HMACSHA512();

			user.UserName = userDto.UserName.ToLower();
			user.PasswordHah = macHash.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
			user.PasswordSalt = macHash.Key;
			
			var checkUser = await _Context.Users.FirstOrDefaultAsync(a => a.UserId == user.UserId);

			await _Context.AddAsync(user);
			await _Context.SaveChangesAsync();
			return Ok
			(
			new UserDTO 
			{
				Token = TokenService.CreateToken(user) , 
				UserName = user.UserName,
				KnownAs = user.KnownAs,
				gender = user.Gender
			});
			
		}

		[HttpPost("login")]

		public async Task<ActionResult<UserDTO>> login(LoginDTO login)
		{
			AppUser user = await _Context.Users
				.Include(p => p.photos)
				.FirstOrDefaultAsync(x => x.UserName ==  login.UserName);
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
				UserName = user.UserName,
				PhotoUrl = user.photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				gender = user.Gender
			});
		}
	}
}
