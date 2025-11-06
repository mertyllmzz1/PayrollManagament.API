using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Models;
using PayrollManagement.Data.Models.DTO;
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
		private readonly IEmployeeRepository _employeeRepo;

		public EmployeeService(IGenericRepository<Employee> repo, IEmployeeRepository employeeRepo)
		{
			_repo = repo;
			_employeeRepo = employeeRepo;
		}

		public Task<int> AddAsync(Employee entity, string spName) => _repo.AddAsync( entity,  spName);
		public Task<IEnumerable<Employee>> GetAllAsync(string spName) => _repo.GetAllAsync(spName);
		public Task<Employee?> GetByIdAsync(string tableName, int id, string idColumn) => _repo.GetByIdAsync( tableName, id,  idColumn);
		public Task<bool> UpdateAsync(Employee entity) => _repo.UpdateAsync("UpdateEmployee", entity); //TODO: generic yapılabilir mi? yoksa kendi classında mı kalmalı?
		public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync("Delete","Id",id);

		public Task<IEnumerable<EmployeePayrollDto>> GetEmployeePeriodicPayroll(string spName, string workPeriod)=>_employeeRepo.GetEmployeePeriodicPayroll( spName,  workPeriod);

	}
}
