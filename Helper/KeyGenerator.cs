using System;
using System.Text;
using System.Security.Cryptography;

namespace DATAspNETApiCoreMVC.Helper
{
	public static class KeyGenerator
	{
		public static string GenerateApiKey(int length = 32)
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var tokenData = new byte[length];
				rng.GetBytes(tokenData);

				return Convert.ToBase64String(tokenData);
			}
		}

		public static string GenerateJwtKey(int length = 32)
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var keyData = new byte[length];
				rng.GetBytes(keyData);

				return Convert.ToBase64String(keyData);
			}
		}

		public static string GenerateIssuer() => $"Issuer_{Guid.NewGuid()}";
		public static string GenerateAudience() => $"Audience_{Guid.NewGuid()}";
	}
}
