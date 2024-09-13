using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DATAspNETApiCoreMVC.Models
{
	public class UserAccountModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // This attribute makes Id auto-increment
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Address { get; set; }
		public string? State { get; set; }
		public string? Organization { get; set; }
		public string? ZipCode { get; set; }
		public string? Country { get; set; }
		public string? Language { get; set; }
		public string? Currency { get; set; }
		public string? UserID { get; set; }
	}
}
