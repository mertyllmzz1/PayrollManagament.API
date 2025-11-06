using Xunit;
using PayrollManagement.Data;
using PayrollManagement.Data.Repositories;
using PayrollManagement.Data.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using PayrollManagement.Data.Models;
using Microsoft.Extensions.Configuration.Json;
using PayrollManagement.Extentions;
using PayrollManagement.Data.Abstracts;
namespace PayrollManagement.Test.Repos
{
	public class GenericRepositoriesTests
	{
		private readonly IGenericRepository<Employee> _repository;

		public GenericRepositoriesTests()
		{
			var config = TestConfigBuilder.BuildConfiguration();

			var context = new DataContext(config);
			_repository = new GenericRepository<Employee>(context);
		}
		[Fact]
		public async Task Should_Get_All_Employees()
		{
			var result = await _repository.GetAllAsync("Employees");
			Assert.NotNull(result);
		}

		[Fact]
		public async Task Should_Add_Employee()
		{
			var newEmployee = new Employee
			{
				IdentityNo = "11111111111",
				Name = "Test",
				Surname = "Tester",
				DailyWage = 800,
				CreatedAt = DateTime.Now,
				Active = true,
				PayrollType = 1
			};

			var affected = await _repository.AddAsync(newEmployee, "AddEmployee");
			Assert.True(affected > 0);
		}

		[Fact]
		public async Task Should_Get_Employee_By_Id()
		{
			var result = await _repository.GetByIdAsync("Employees", 2, "Id");
			Assert.NotNull(result);
		}

		[Fact]
		public async Task Should_Update_Employee()
		{
			var existingEmployee = new Employee
			{
				Id= 2,
				IdentityNo = "11111111111",
				Name = "Test",
				Surname = "Tester",
				CreatedAt = Convert.ToDateTime("05/11/2025"),
				DailyWage = 1000,
				Active = true,
				PayrollType = 2
			};

			var affected = await _repository.UpdateAsync("UpdateEmployee",existingEmployee);
			Assert.True(affected);
		}
	}
}
