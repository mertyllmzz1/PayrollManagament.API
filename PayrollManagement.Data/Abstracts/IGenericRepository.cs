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
		Task<IEnumerable<T>> GetAllAsync(string spName);
		Task<T?> GetByIdAsync(string tableName, int id, string idColumn);
		Task<int> AddAsync(T entity, string spName);
		Task<bool> UpdateAsync(string spName, T entity);
		Task<bool> DeleteAsync(string spName, string idColumn, int id);
	}
}
