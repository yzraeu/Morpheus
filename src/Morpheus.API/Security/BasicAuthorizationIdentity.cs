using System.Security.Principal;

namespace Morpheus.API.Security
{
	public class BasicAuthorizationIdentity : GenericIdentity
    {
		public string Password { get; set; }

		public BasicAuthorizationIdentity(string username, string password) : base(username, "Basic")
		{
			this.Password = password;
		}
	}
}
