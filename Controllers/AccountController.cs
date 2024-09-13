using DATAspNETApiCoreMVC.Data;
using DATAspNETApiCoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATAspNETApiCoreMVC.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly ApplicationDbContext _context;


		public AccountController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserAccountModel>>> GetUserAccounts()
		{
			return await _context.AspNetUserAccounts.ToListAsync();
		}

		[HttpGet("{UserId}")]
		[Authorize] // Chỉ cho phép truy cập khi có token hợp lệ
		public async Task<ActionResult<UserAccountModel>> GetUserAccount(string UserId)
		{

			var userAccount = await _context.AspNetUserAccounts.SingleOrDefaultAsync(ua => ua.UserID == UserId); // Use FirstOrDefaultAsync here


			if (userAccount == null)
			{
				return NotFound();
			}

			return userAccount;
		}

		[HttpPost]
		public async Task<ActionResult<UserAccountModel>> PostUserAccount(UserAccountModel userAccount)
		{
			_context.AspNetUserAccounts.Add(userAccount);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUserAccount", new { id = userAccount.UserID }, userAccount);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutUserAccount(string id, UserAccountModel userAccount)
		{
			if (id != userAccount.UserID)
			{
				return BadRequest();
			}

			_context.Entry(userAccount).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserAccountExists(id.ToString()))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUserAccount(string id)
		{
			var userAccount = await _context.AspNetUserAccounts.FindAsync(id);
			if (userAccount == null)
			{
				return NotFound();
			}

			_context.AspNetUserAccounts.Remove(userAccount);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool UserAccountExists(string id)
		{
			return _context.AspNetUserAccounts.Any(e => e.UserID == id);
		}
	}
}
