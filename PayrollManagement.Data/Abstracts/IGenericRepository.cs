using PayrollManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Data.Abstracts
{
	public interface IGenericRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync(string tableName);
		Task<T?> GetByIdAsync(string tableName, int id, string idColumn);
		Task<int> AddAsync(string tableName, Dictionary<string, object> parameters);
		Task<bool> UpdateAsync(string tableName, Dictionary<string, object> parameters, string idColumn, int id);
		Task<bool> DeleteAsync(string tableName, string idColumn, int id);
	}
}
