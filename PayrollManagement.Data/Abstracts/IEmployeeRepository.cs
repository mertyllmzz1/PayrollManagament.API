using PayrollManagement.Data.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Data.Abstracts
{
	public interface IEmployeeRepository
	{
		Task<IEnumerable<EmployeePayrollDto>> GetEmployeePeriodicPayroll(string spName, string month);
	}
}
