using DATAspNETApiCoreMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly IConfiguration _configuration;


	public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_configuration = configuration;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterModel model)
	{
		var user = new ApplicationUser
		{
			UserName = model.Email,
			Email = model.Email,
			ApiKey = GenerateApiKey()
		};
		var result = await _userManager.CreateAsync(user, model.Password);
		if (result.Succeeded)
		{
			return Ok(new { ApiKey = user.ApiKey});
		}

		return BadRequest(result.Errors);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginModel model)
	{
		var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
		if (result.Succeeded)
		{
			var appUser = await _userManager.FindByEmailAsync(model.Email);
			if (appUser != null)
			{
				return Ok(new { Token = GenerateJwtToken(appUser) });
			}
		}

		return Unauthorized("Invalid login attempt.");
	}

	private string GenerateJwtToken(ApplicationUser user)
	{
		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Email),
			new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim("api_key", user.ApiKey.ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _configuration["Jwt:Issuer"],
			audience: _configuration["Jwt:Audience"],
			claims: claims,
			expires: DateTime.Now.AddMinutes(60),
			signingCredentials: creds);
		string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

		return tokenValue;
	}

	private string GenerateApiKey()
	{
		return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "").Replace("/", "");
	}
}