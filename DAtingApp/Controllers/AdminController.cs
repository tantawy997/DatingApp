using DatingApp.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace DAtingApp.Controllers
{

	public class AdminController : ApiControllerBase
	{
		private readonly UserManager<AppUser> _UserManager;

		public AdminController(UserManager<AppUser> userManager)
		{
			_UserManager = userManager;
		}
		[Authorize(Policy = "RequireAnAdminRole")]
		[HttpGet("Users-With-Roles")]
		public async Task<ActionResult> GetUsersWithRoles()
		{
			var users = await _UserManager.Users.OrderBy(u => u.UserName).Select(u => new
			{
				u.Id,
				u.UserName,
				roles = u.UserRoles.Select(role => role.Role.Name).ToList()
			}).ToListAsync();

			return Ok(users);
		}

		[Authorize(Policy = "ModeratePhotoRole")]
		[HttpGet("photo-to-moderate")]
		public ActionResult GetPhotosForModerators()
		{
			return Ok("only admins or moderators can see this");
		}

		[Authorize(Policy = "RequireAnAdminRole")]
		[HttpPost("edit-roles/{username}")]
		public async Task<ActionResult> EditRoles(string username, [FromQuery] string Roles)
		{
			if (string.IsNullOrEmpty(Roles)) return BadRequest("you must select at least one role");

			var selectRoles = Roles.Split(",").ToArray();

			var user = await _UserManager.FindByNameAsync(username);
			if (user == null) return NotFound();

			var UserRoles = await _UserManager.GetRolesAsync(user);

			 var result = await _UserManager.AddToRolesAsync(user,selectRoles.Except(UserRoles));

			if (!result.Succeeded) return BadRequest("failed to add to roles");

			result = await _UserManager.RemoveFromRolesAsync(user,UserRoles.Except(selectRoles));

			if (!result.Succeeded) return BadRequest("failed to remove to roles");

			return Ok(await _UserManager.GetRolesAsync(user));
		}

	}
}
