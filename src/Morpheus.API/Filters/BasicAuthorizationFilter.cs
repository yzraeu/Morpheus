using Morpheus.API.Security;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;

namespace Morpheus.API.Filters
{
	public class BasicAuthorizationFilter : IAuthorizationFilter
	{
		private readonly ILogger<BasicAuthorizationFilter> _logger;
		private readonly Configurations.APIAuthorization _authorizationConfig;

		public BasicAuthorizationFilter(ILogger<BasicAuthorizationFilter> logger, IOptions<Configurations.APIAuthorization> authorizationConfig)
		{
			_logger = logger;
			_authorizationConfig = authorizationConfig.Value;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var authorizationData = ParseHeaderInformation(context);

			if (authorizationData == null) throw new UnauthorizedAccessException();

			if (!IsAuthorizationValid(authorizationData.Name, authorizationData.Password)) throw new UnauthorizedAccessException();

			var identity = new ClaimsIdentity(authorizationData);

			var principal = new ClaimsPrincipal(identity);

			context.HttpContext.User = principal;
		}

		private bool IsAuthorizationValid(string username, string password)
		{
			if (_authorizationConfig.Password.Equals(password) && _authorizationConfig.Username.Equals(username))
				return true;
			else
				return false;
		}

		private BasicAuthorizationIdentity ParseHeaderInformation(AuthorizationFilterContext context)
		{
			var authValue = context.HttpContext.Request.Headers["Authorization"];
			if (string.IsNullOrEmpty(authValue) || authValue.Count == 0) return null;

			if (!authValue[0].Contains("Basic ")) return null;

			var authBase64Value = authValue.ToString().Split(' ')[1];

			var authHeader = Encoding.UTF8.GetString(Convert.FromBase64String(authBase64Value));

			var tokens = authHeader.Split(':');
			if (tokens.Length != 2) return null;

			return new BasicAuthorizationIdentity(tokens[0], tokens[1]);
		}
	}
}
