using Microsoft.AspNetCore.Identity;

namespace DATAspNETApiCoreMVC.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? Username { get; set; }
		public string? ApiKey { get; set; }
	}
}
