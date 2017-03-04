using Morpheus.Domain.Entities;
using Morpheus.Service;
using Morpheus.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Morpheus.API.Controllers
{
	[Authorize]
	[Route("v1/[controller]")]
	public class UsersController : Controller
	{
		private readonly ILogger _logger;
		private readonly UserService _userService;

		public UsersController(ILogger<UsersController> logger, UserService userService)
		{
			_logger = logger;
			_userService = userService;
		}

		[HttpGet]
		public async Task<IList<User>> Get()
		{
			return await _userService.GetAll();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{ 
			return Ok(await _userService.Get(id));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]User user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.Add(user);

			return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody]User user)
		{
			user.Id = id;
			
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.Update(user);

			return AcceptedAtAction(nameof(Get), user.Id);
		}

		[HttpPut("{id}/Address")]
		public async Task<IActionResult> PutAddress(int id, [FromBody]Domain.ViewModels.Request.UpdateAddress updateVM)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			await _userService.UpdateAddress(id, updateVM);

			return AcceptedAtAction(nameof(Get), id);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _userService.Remove(id);

			return Ok();
		}
	}
}
