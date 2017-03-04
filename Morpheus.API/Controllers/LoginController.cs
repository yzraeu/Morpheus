using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Morpheus.Service;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Morpheus.Domain.DTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Morpheus.API.Controllers
{
	[Authorize]
    public class LoginController : Controller
    {
		private readonly ILogger _logger;
		private readonly UserService _userService;

		public LoginController(ILogger<UsersController> logger, UserService userService)
		{
			_logger = logger;
			_userService = userService;
		}

		[HttpPost]
		[Route("v1/Login")]
		public async Task<IActionResult> Login()
		{
			// Get Token info
			var authValue = HttpContext.Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(authValue) || authValue.Count == 0) return null;

			if (!authValue[0].Contains("Bearer ")) return null;

			var jwtToken = authValue.ToString().Split(' ')[1];

			var rawPayload = jwtToken.Split('.')[1];

			var jsonPayload = Domain.Util.Security.Base64Decode(rawPayload);

			var tokenPayload = JsonConvert.DeserializeObject<TokenPayloadDTO>(jsonPayload);

			return Ok(await _userService.Login(tokenPayload));
		}
	}
}
