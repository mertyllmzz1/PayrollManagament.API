using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Models;
using PayrollManagement.Data.Models.DTO;
using PayrollManagement.Extentions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Data.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly DataContext _context;
		private readonly IDbConnection _connection;
		public EmployeeRepository(DataContext context)
		{
			_context = context;
			_connection = _context.CreateConnection();
		}
		public async Task<IEnumerable<EmployeePayrollDto>> GetEmployeePeriodicPayroll(string spName, string month)
		{
			if (_connection.State != ConnectionState.Open)
			{
				if (_connection is System.Data.Common.DbConnection dbConn)
					await dbConn.OpenAsync();
				else
					_connection.Open();
			}
			using var command = new SqlCommand($"Exec {spName} @WorkPeriod= '{month}'", (SqlConnection)_connection);

			var list = new List<EmployeePayrollDto>();
			using var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				var entity = new EmployeePayrollDto
				{
					Id = reader.GetInt32(reader.GetOrdinal("Id")),
					Name = reader["Name"].ToString() ?? "",
					Surname = reader["Surname"].ToString() ?? "",
					PayrollTypeDescription = reader["PayrollTypeDescription"].ToString() ?? "",
					DailyWage = Convert.ToDecimal(reader["DailyWage"]),
					PeriodicWage = Convert.ToDecimal(reader["PeriodicWage"]),
					IdentityNo = reader["IdentityNo"].ToString() ?? "",
					OvertimeWork = reader.GetInt32(reader.GetOrdinal("OvertimeWork")),
					TotalWorkDay = reader.GetInt32(reader.GetOrdinal("TotalWorkDay")),
				};


			list.Add(entity);
			}

			return list;
		}
	}
}
