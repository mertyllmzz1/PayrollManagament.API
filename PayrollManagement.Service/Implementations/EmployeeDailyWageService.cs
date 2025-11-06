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
	public class EmployeeDailyWageService : IEmployeeDailyWageService
	{
		private readonly IGenericRepository<EmployeeDailyWage> _repo;
		private readonly IEmployeeRepository _employeeRepo;

		public EmployeeDailyWageService(IGenericRepository<EmployeeDailyWage> repo, IEmployeeRepository employeeRepo)
		{
			_repo = repo;
			_employeeRepo = employeeRepo;
		}

		public Task<int> AddAsync(EmployeeDailyWage entity, string spName) => _repo.AddAsync(entity, spName);
		public Task<IEnumerable<EmployeeDailyWage>> GetAllAsync(string spName) => _repo.GetAllAsync(spName);
		public Task<bool> UpdateAsync(EmployeeDailyWage entity) => _repo.UpdateAsync("sp_UpdateEmployee", entity); //TODO: generic yapılabilir mi? yoksa kendi classında mı kalmalı?
		public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync("Delete", "Id", id);

	}
}
