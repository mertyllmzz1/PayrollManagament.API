using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Data
{
	public class DataContext
	{
		private readonly IConfiguration _config;
		private readonly string _connectionString;

		public DataContext(IConfiguration config )
		{
			_config = config;
			_connectionString = _config.GetConnectionString("default") ?? throw new InvalidOperationException("Connection string 'default' not found.");
		}

		public IDbConnection CreateConnection()
		{
			return new Microsoft.Data.SqlClient.SqlConnection(_connectionString);
		}
	}
}
