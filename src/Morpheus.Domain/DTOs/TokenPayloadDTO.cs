using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Morpheus.Domain.DTOs
{
    public class TokenPayloadDTO
	{
		public string iss { get; set; }
		public string name { get; set; }
		public string picture { get; set; }
		public string aud { get; set; }
		public long auth_time { get; set; }
		public string user_id { get; set; }
		public string sub { get; set; }
		public long iat { get; set; }
		public long exp { get; set; }
		public string email { get; set; }
		public bool email_verified { get; set; }
	}
}
