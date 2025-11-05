using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Models;
using PayrollManagement.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Service.Implementations
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IGenericRepository<Employee> _repo;
		public EmployeeService(IGenericRepository<Employee> repo)
		{
			_repo = repo;
		}

		public Task<int> AddAsync(Employee entity, string spName) => _repo.AddAsync(entity, "AddEmployee");
		public Task<IEnumerable<Employee>> GetAllAsync(string spName) => _repo.GetAllAsync("GetEmployees");
		public Task<Employee?> GetByIdAsync(string tableName, int id, string idColumn) => _repo.GetByIdAsync("GetEmployees", id, "Id");
		public Task<bool> UpdateAsync(Employee entity) => _repo.UpdateAsync("UpdateEmployee", entity); //TODO: generic yapılabilir mi? yoksa kendi classında mı kalmalı?
		public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync("Delete","Id",id);

		public Task<Employee> GetCustomersPeriodicPayroll(string workPeriod)
		{
			throw new NotImplementedException();
		}

	}
}
