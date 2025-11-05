using PayrollManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Service.Abstracts
{
	public interface IEmployeeService:IGenericService<Employee>
	{
		Task<Employee> GetCustomersPeriodicPayroll(string workPeriod);
	}
}
