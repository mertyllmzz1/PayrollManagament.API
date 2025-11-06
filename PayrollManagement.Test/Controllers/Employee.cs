
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;
using PayrollManagament.API.Controllers;
using PayrollManagement.Data.Models;
using PayrollManagement.Data.Models.DTO;
using PayrollManagement.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;

namespace PayrollManagement.Test.Controllers
{
	public class Employee: IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly Mock<IEmployeeService> _mockEmployeeService;
		private readonly Mock<ILogger<EmployeeController>> _mockLogger;
		private readonly Mock<IEmployeeDailyWageService> _mockEmployeeWageService;
		private readonly EmployeeController _controller;

		private readonly WebApplicationFactory<Program> _factory;
		private readonly HttpClient _client;
		public Employee(WebApplicationFactory<Program> factory)
		{
			_mockEmployeeService = new Mock<IEmployeeService>();
			_mockLogger = new Mock<ILogger<EmployeeController>>();
			_mockEmployeeWageService = new Mock<IEmployeeDailyWageService>();
			_controller = new EmployeeController(_mockEmployeeService.Object, _mockLogger.Object, _mockEmployeeWageService.Object);
			_factory = factory;
			_client = _factory.CreateClient();
		}
		[Fact]
		public async Task GetAll_Employees_Test()
		{
			var token = await GetJwtTokenAsync();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

	
			var response = await _client.GetAsync("api/Employee/employees");
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var employees = JsonConvert.DeserializeObject<List<EmployeePayrollDto>>(content);
			Assert.True(employees.Count > 1); 
			//Assert.Contains(employees, e => e.Id == 1); 
	

		}
		
		[Fact]
		public async Task Create_Test()
		{
			var token = await GetJwtTokenAsync();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var random = new Random();
			int uniqueNumber = random.Next(1000, 10000);
			var newEmployee = new Data.Models.Employee
			{
				IdentityNo = String.Concat("1111111", uniqueNumber.ToString()),
				Name = "Test",
				Surname = "Tester",
				DailyWage = 800,
				CreatedAt = DateTime.Now,
				Active = true,
				PayrollType = 1
			};
			var response = await _client.PostAsJsonAsync("api/Employee/employees",newEmployee);

			response.EnsureSuccessStatusCode(); 
			var content = await response.Content.ReadAsStringAsync();
			Assert.True(response.IsSuccessStatusCode);
		}
		[Fact]
		public async Task GetPayrolls_Test()
		{
			var token = await GetJwtTokenAsync();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


			var response = await _client.GetAsync("/api/employee/payrolls/2025-11");
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var employees = JsonConvert.DeserializeObject<List<EmployeePayrollDto>>(content);
			Assert.True(employees.Count > 1);
			//Assert.Contains(employees, e => e.Id == 1); 
		}
		[Fact]
		public async Task PostPayrolls_Test()
		{
			var token = await GetJwtTokenAsync();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var newWorkHistoryRecord = new Data.Models.EmployeeDailyWage
			{
				 DailyTotalWorkingMinute = 540,
				 EmployeeId =11,
				 Day = 15,
				 Month=11,
				 Year= 2025,
				
			};
			var response = await _client.PostAsJsonAsync("api/Employee/payrolls", newWorkHistoryRecord);

			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			Assert.True(response.IsSuccessStatusCode);
		}
		[Fact]
		public async Task GetWorkHistory_Test()
		{
			var token = await GetJwtTokenAsync();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


			var response = await _client.GetAsync("api/Employee/worklogs");
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			var employees = JsonConvert.DeserializeObject<List<EmployeeDailyWage>>(content);
			Assert.True(employees.Count > 1);
			//Assert.Contains(employees, e => e.Id == 1); 
		}
		private async Task<string> GetJwtTokenAsync()
		{
			var loginData = new LoginRequest
			{
				Email = "admin",
				Password = "admin123" // test kullanıcın
			};

			var response = await _client.PostAsJsonAsync("/login", loginData);
			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();
			dynamic result = JsonConvert.DeserializeObject(content);
			return (string)result.access_token;
		}
	}
}
