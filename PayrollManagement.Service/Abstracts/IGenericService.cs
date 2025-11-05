using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Service.Abstracts
{
	public interface IGenericService<T> where T : class
	{
		Task<int> AddAsync(T entity ,string spName);
		Task<IEnumerable<T>> GetAllAsync(string spName);
		Task<T?> GetByIdAsync(string tableName , int id, string idColumn);
		Task<bool> UpdateAsync(T entity);
		Task<bool> DeleteAsync(int id);
	}
}
