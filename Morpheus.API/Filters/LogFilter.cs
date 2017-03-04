using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Morpheus.API.Filters
{
	public class LogFilter : ActionFilterAttribute
	{
		private readonly ILogger<LogFilter> _logger;

		private Stopwatch sw;
		public LogFilter(ILogger<LogFilter> logger)
		{
			_logger = logger;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			base.OnActionExecuting(context);
			_logger.LogDebug("START REQUEST -------------------------------------------------");
			sw = Stopwatch.StartNew();
			_logger.LogDebug($"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}");

			var headers = context.HttpContext.Request.Headers;

			// Detail the info inside header
			foreach (var header in headers)
				_logger.LogDebug($" - {header.Key}: {header.Value}");

			_logger.LogDebug($"BEGIN {context.Controller}");
			_logger.LogDebug($"============================================================");
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
			_logger.LogDebug($"============================================================");
			_logger.LogDebug($"FINISHED {context.Controller}");

			var requestElapsedMs = sw.ElapsedMilliseconds;
			if (context.Exception != null)
				_logger.LogCritical("ActionError: " + context.Exception.Message);


			if (context.HttpContext.Response != null)
				_logger.LogDebug($"Response is {context.HttpContext.Response.StatusCode}");

			_logger.LogDebug($"Elapsed time: {requestElapsedMs} ms");
			_logger.LogDebug("END REQUEST -------------------------------------------------");
		}
	}
}
