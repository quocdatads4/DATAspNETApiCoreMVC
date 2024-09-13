using Microsoft.AspNetCore.Identity;

namespace DATAspNETApiCoreMVC.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string? ApiKey { get; set; }
	}
}
