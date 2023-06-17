using DatingApp.Data;
using DatingApp.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAtingApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ApiControllerBase
	{
		private readonly DataContext _Context;

		public AccountController(DataContext Context)
		{
			_Context = Context;
		}

		//[HttpPost("Register")]

		//public async Task<ActionResult<AppUser>> Register(AppUser user)
		//{
		//	return await Ok(_Context.SaveChangesAsync());
		//}
	}
}
