using DatingApp.Data;
using DatingApp.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAtingApp.Controllers
{
	//[Route("api/[controller]")]
	//[ApiController]
	public class BugController : ApiControllerBase
	{
		private readonly DataContext _Context;

		public BugController(DataContext Context)
		{
			_Context = Context;
		}

		[Authorize]
		[HttpGet("UnAuthorized")]
		public ActionResult<string> getSecret()
		{
			return "secret-text";
		}

		[HttpGet("not-Fount")]
		public ActionResult<AppUser> GetNotFound()
		{
			var thing = _Context.Users.Find(-1);

			if (thing == null) return NotFound();

			return Ok(thing);
		}
		[HttpGet("bad-request")]
		public ActionResult<string> GetBadRequest()
		{
			return BadRequest();
		}

		[HttpGet("server-error")]
		public ActionResult<string> GetServerError()
		{
			var thing = _Context.Users.Find(-1);

			var badReq = thing.ToString();

			return badReq;
		}

	}
}
