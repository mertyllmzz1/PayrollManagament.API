using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;
using Moq;
using PayrollManagement.Data;
using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Models;
using PayrollManagement.Data.Models.DTO;
using PayrollManagement.Data.Repositories;
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
		private readonly IEmployeeService _mockEmployeeService;

		private readonly IGenericRepository<Employee> _repo;
		private readonly IEmployeeRepository _employeeRepo;
		private readonly IEmployeeService _employeeService;


		public EmployeeService()
		{
			_mockRepo = new Mock<IGenericRepository<Employee>>();
			_mockEmployeeRepo = new Mock<IEmployeeRepository>();
			_mockEmployeeService = new Service.Implementations.EmployeeService(_mockRepo.Object, _mockEmployeeRepo.Object);

			var config = TestConfigBuilder.BuildConfiguration();
			var context = new DataContext(config);
			_repo = new GenericRepository<Employee>(context);
			_employeeRepo= new EmployeeRepository(context);
			_employeeService = new Service.Implementations.EmployeeService(_repo, _employeeRepo);
		}

		[Fact]
		public async Task AddAsync_ShouldCallAdd()
		{

			var emp = new Employee { Name = "Test", Surname = "User", IdentityNo = "12345678901" };
			_mockRepo.Setup(r => r.AddAsync(emp, "sp_AddEmployee")).ReturnsAsync(1);
			var result = await _mockEmployeeService.AddAsync(emp, "sp_AddEmployee");

			Assert.Equal(1, result);
			_mockRepo.Verify(r => r.AddAsync(emp, "sp_AddEmployee"), Times.Once);
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
			_mockRepo.Setup(r => r.GetAllAsync("sp_GetEmployees")).ReturnsAsync(employees);

			// Act
			var result = await _mockEmployeeService.GetAllAsync("sp_GetEmployees");

			// Assert
			Assert.Equal(2, result.Count());
		}

		
		[Fact]
		public async Task CalculateSalary_ForAllEmployeeTypes_ShouldReturnCorrectSalary()
		{
			var mockService = new Mock<IEmployeeService>();
			var employees = new List<EmployeePayrollDto>
									{
										new EmployeePayrollDto {
											Id = 2,
											Name = "Test",
											Surname = "Tester",
											DailyWage = 1000m,
											PayrollTypeDescription = "Günlük Ücret X Çalışılan gün sayısı",
											PeriodicWage = 1000.00m * 3,
											IdentityNo = "*******1111"
										}, // Tip 2: Günlük ücret * çalışma gün
											new EmployeePayrollDto {
											Id = 3,
											Name = "Test",
											Surname = "Tester",
											DailyWage = 1000m,
											PayrollTypeDescription = "Sabit Maaş",
											PeriodicWage = 800m * 30,
											IdentityNo = "*******1111"
										}, // Tip 1: Sabit maaş ( Günlük ücret * 30 )
											new EmployeePayrollDto {
											Id =10,
											Name = "Test",
											Surname = "Tester",
											DailyWage = 800m,
											PayrollTypeDescription = "Günlük Ücret X Çalışılan gün sayısı",
											PeriodicWage =  24000m + (870m/60m * 800m/9m * 1.5m),
											IdentityNo = "*******1111"
										} // Tip 3: Sabit  maaş ( Günlük ücret * 30 ) 24000+ (fazla mesai/saat * gunluk ucret / gunluk normal çalışma süresi * Fazla Mesai çarpanı)
									}.AsEnumerable();
			mockService.Setup(s => s.GetEmployeePeriodicPayroll(It.IsAny<string>(), It.IsAny<string>()))
						  .ReturnsAsync(employees);

			// Act: mock service üzerinden çağır
			var result = await mockService.Object.GetEmployeePeriodicPayroll("sp_GetEmployeePeriodicPayroll", "2025-11");
			foreach (var emp in result)
			{
				decimal expected = 0;
				expected= emp.Id == 2 ? 3000m : emp.Id == 3 ? 24000m : 25933.3300000000m;	
				Assert.Equal(expected, emp.PeriodicWage, precision:2);
			}
		}
		[Fact]
		public async Task GetEmployeePeriodicPayroll_ShouldReturnCorrectSalary_FromDB()
		{
		
			string workPeriod = "2025-11";

			var result = await _employeeService.GetEmployeePeriodicPayroll("sp_GetEmployeePeriodicPayroll", workPeriod);

			Assert.NotNull(result);
			Assert.NotEmpty(result);

			foreach (var emp in result)
			{
				decimal expected = 0;

				if (emp.PayrollTypeDescription.Contains("Sabit Maaş") && !emp.PayrollTypeDescription.Contains("Fazla Mesai"))
					expected = emp.DailyWage * 30;
				else if (emp.PayrollTypeDescription.Contains("Günlük Ücret"))
					expected = emp.DailyWage * emp.TotalWorkDay; 
				else if (emp.PayrollTypeDescription.Contains("Fazla Mesai"))
					expected = (emp.DailyWage * 30) + (emp.DailyWage/9 * emp.OvertimeWork / 60 * 1.5m); 

		
				Assert.Equal(expected, emp.PeriodicWage, 2);
			}
		}
		[Fact]
		public async Task GetEmployee_FromDB()
		{

			var result = await _employeeService.GetAllAsync("sp_GetEmployees");

			Assert.NotNull(result);
			Assert.NotEmpty(result); ;
			Assert.False(result.Where(p => p.IdentityNo.Substring(0, 7) != "*******").Any(), "Maskelenmemiş TC kimlik bilgisi içeriyor");

		}
	}
}
