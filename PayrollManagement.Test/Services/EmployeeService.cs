using Moq;
using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Models;
using PayrollManagement.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Test.Services
{
	public class EmployeeService
	{
		private readonly Mock<IGenericRepository<Employee>> _mockRepo;
		private readonly Mock<IEmployeeRepository> _mockEmployeeRepo;
		private readonly IEmployeeService _employeeService;

		public EmployeeService()
		{
			_mockRepo = new Mock<IGenericRepository<Employee>>();
			_mockEmployeeRepo = new Mock<IEmployeeRepository>();
			_employeeService = new Service.Implementations.EmployeeService(_mockRepo.Object,_mockEmployeeRepo.Object);
		}

		[Fact]
		public async Task AddAsync_ShouldCallAdd()
		{
			
			var emp = new Employee { Name = "Test", Surname = "User", IdentityNo = "12345678901" };
			_mockRepo.Setup(r => r.AddAsync(emp, "AddEmployee")).ReturnsAsync(1);		
			var result = await _employeeService.AddAsync(emp,"AddEmployee");
			
			Assert.Equal(1, result);
			_mockRepo.Verify(r => r.AddAsync(emp, "AddEmployee"), Times.Once);
		}
		[Fact]
		public async Task GetAllAsync_ShouldReturnAllEmployees()
		{
			// Arrange
			var employees = new List<Employee>
			{
				new Employee { Id = 1, Name = "A", Surname = "B", IdentityNo = "11111111111" },
				new Employee { Id = 2, Name = "C", Surname = "D", IdentityNo = "22222222222" }
			};
			_mockRepo.Setup(r => r.GetAllAsync("GetEmployees")).ReturnsAsync(employees);

			// Act
			var result = await _employeeService.GetAllAsync("GetEmployees");

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnEmployee()
		{
			var emp = new Employee { Id = 1, Name = "Test", Surname = "User", IdentityNo = "12345678901" };
			_mockRepo.Setup(r => r.GetByIdAsync("GetEmployees",1,"Id")).ReturnsAsync(emp);

			var result = await _employeeService.GetByIdAsync("GetEmployees", 1, "Id");
			Assert.NotNull(result);
			Assert.Equal("Test", result.Name);
		}
	}
}
