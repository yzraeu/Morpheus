using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Morpheus.API.Filters
{
	public class ModelValidatorFilter : ActionFilterAttribute
    {
		private readonly ILogger<ModelValidatorFilter> _logger;

		public ModelValidatorFilter(ILogger<ModelValidatorFilter> logger)
		{
			_logger = logger;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				_logger.LogInformation("ModelState is NOT valid");

				context.Result = new BadRequestObjectResult(context.ModelState);
			}
		}
	}
}
