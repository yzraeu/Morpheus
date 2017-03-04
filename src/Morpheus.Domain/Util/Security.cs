using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Morpheus.Domain.Util
{
    public static class Security
    {
		public static string HashString(string unsafeValue)
		{
			byte[] data = System.Text.Encoding.ASCII.GetBytes(unsafeValue);

			using (var algorithm = SHA256.Create())
			{
				data = algorithm.ComputeHash(data);
			}
			return System.Text.Encoding.ASCII.GetString(data);
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData)
		{
			int mod4 = base64EncodedData.Length % 4;
			if (mod4 > 0)
				base64EncodedData += new string('=', 4 - mod4);

			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}
