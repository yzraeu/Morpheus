using Morpheus.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Morpheus.API.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly ILogger<ExceptionHandlerMiddleware> _logger;
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
		{
			_logger = logger;
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());

				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var statusCode = HttpStatusCode.InternalServerError;

			if (exception is UnauthorizedAccessException) statusCode = HttpStatusCode.Unauthorized;
			else if (exception is NotFoundException) statusCode = HttpStatusCode.NotFound;
			else if (exception is ArgumentException) statusCode = HttpStatusCode.BadRequest;
			else if (exception is ArgumentNullException) statusCode = HttpStatusCode.BadRequest;
			else if (exception is ArgumentOutOfRangeException) statusCode = HttpStatusCode.BadRequest;
			else if (exception is InvalidParameterException) statusCode = HttpStatusCode.BadRequest;
			else if (exception is InvalidDeviceException) statusCode = HttpStatusCode.BadRequest;
			else statusCode = HttpStatusCode.InternalServerError;

			return WriteExceptionAsync(context, exception, statusCode);
		}

		private static Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
		{
			var response = context.Response;
			response.ContentType = "application/json";
			response.StatusCode = (int)statusCode;
			return response.WriteAsync(JsonConvert.SerializeObject(new
			{
				error = new
				{
					message = exception.Message,
					exception = exception.GetType().Name
				}
			}));
		}

	}
}
