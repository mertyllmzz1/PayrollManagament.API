using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayrollManagement.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace PayrollManagement.Service.Implementations
{
	public class AuthanticationService : IAuthanticationService
	{
		private readonly IConfiguration _config;
		public AuthanticationService(IConfiguration config) => _config = config;
		public string CreateToken(string userId, string username, IEnumerable<Claim>? extraClaims = null)
		{
			var jwtSection = _config.GetSection("Jwt");
			var key = jwtSection.GetValue<string>("Key")!;
			var issuer = jwtSection.GetValue<string>("Issuer")!;
			var audience = jwtSection.GetValue<string>("Audience")!;
			var expireMinutes = jwtSection.GetValue<int>("ExpireMinutes");

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(ClaimTypes.Name, username),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			if (extraClaims != null)
				claims.AddRange(extraClaims);

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expireMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
