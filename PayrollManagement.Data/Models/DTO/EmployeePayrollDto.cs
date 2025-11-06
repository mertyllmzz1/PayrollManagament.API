using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Data.Models.DTO
{
	public class EmployeePayrollDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Surname { get; set; } = null!;
		public string IdentityNo { get; set; } = null!;
		public string PayrollTypeDescription { get; set; } = null!;
		public decimal DailyWage { get; set; }
		public decimal PeriodicWage { get; set; }

	}
}
