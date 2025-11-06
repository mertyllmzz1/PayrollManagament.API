using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Service.Abstracts
{
	public interface IAuthanticationService
	{
		string CreateToken(string userId, string username, IEnumerable<Claim>? extraClaims = null);
	}
}
