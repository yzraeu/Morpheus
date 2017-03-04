using Morpheus.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Morpheus.API.Controllers
{
	[Route("v1/[controller]")]
	public class TestController : Controller
	{
		private readonly ILogger<UsersController> _logger;

		public TestController(ILogger<UsersController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public async Task<string> Get()
		{
			var sw = Stopwatch.StartNew();
			try
			{
				_logger.LogDebug($"Get");
				return await Task.Run(() => "success");
			}
			catch (System.Exception)
			{
				throw;
			}
			finally
			{
				_logger.LogDebug($"Get Took: {sw.ElapsedMilliseconds} ms");
			}
		}
	}
}
