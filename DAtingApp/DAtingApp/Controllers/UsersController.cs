using DatingApp.Data;
using DatingApp.Entites;
using DAtingApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controller
{
	public class UsersController : ApiControllerBase
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
		[Authorize]
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


	}

}
