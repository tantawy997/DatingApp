using DAtingApp.helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DAtingApp.Controllers
{
	[ServiceFilter(typeof(OnActionExcutionAsync))]
	[Route("api/[controller]")]
	[ApiController]
	public class ApiControllerBase : ControllerBase
	{

	}
}
