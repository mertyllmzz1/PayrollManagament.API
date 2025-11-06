using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PayrollManagement.Service.Abstracts;
using System.Security.Claims;

namespace PayrollManagament.API.Controllers
{
	public class AuthController : ControllerBase
	{
		private readonly IAuthanticationService _authService;
		private readonly IConfiguration _configuration;
		// Basit in-memory kullanıcı (örnek)
		private readonly List<(string UserId, string Email, string Password, string Role)> _users
			= new()
		{
		("1","admin","admin123","Admin"),
		};

		public AuthController(IAuthanticationService authService ,IConfiguration configuration) {
			_authService = authService;
			_configuration = configuration;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginRequest model)
		{
			var user = _users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
			if (user == default) return Unauthorized(new { message = "Invalid credentials" });

			var extraClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Role, user.Role)
		};

			var token = _authService.CreateToken(user.UserId, user.Email, extraClaims);

			return Ok(new
			{
				access_token = token,
				token_type = "Bearer",
				expires_in_minutes = int.Parse(_configuration["Jwt:ExpireMinutes"]) // opsiyonel
			});
		}
	}
}
