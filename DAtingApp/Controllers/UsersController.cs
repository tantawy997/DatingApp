using DatingApp.Data;
using DatingApp.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controller
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly DataContext _Context;

		public UsersController(DataContext dataContext)
		{
			_Context = dataContext;
		}


		[HttpGet("GetUsers")]

		public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
		{
			IEnumerable<AppUser> Users = await _Context.Users.ToListAsync();

			return Ok(Users);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<AppUser>> GetUser(int id)
		{
			if(id == 0)
			{
				return BadRequest();
			}
			AppUser User =await _Context.Users.FirstOrDefaultAsync(u => u.id == id);
			
			if(User == null)
			{
				return NotFound();
			}
			return Ok(User);

		}
		[HttpPost("CreateUser")]

		public async Task<ActionResult<AppUser>> CreateUser(AppUser appUser)
		{
			AppUser User = await _Context.Users.FirstOrDefaultAsync(u => u.id == appUser.id);
			if (User != null)
			{
				return BadRequest();
			}

			var user= await _Context.Users.AddAsync(appUser);
			await _Context.SaveChangesAsync();

			return Ok(user);
		}
	}

}
