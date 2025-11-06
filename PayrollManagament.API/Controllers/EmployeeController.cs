using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollManagement.Data.Models;
using PayrollManagement.Service.Abstracts;

namespace PayrollManagament.API.Controllers
{
	//[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;
		private readonly IEmployeeDailyWageService _dailyWageService;
		//private readonly IGenericService<EmployeeDailyWage> _genericService;
		private readonly ILogger<EmployeeController	> _logger;
		public EmployeeController( IEmployeeService employeeService, ILogger<EmployeeController> logger , IEmployeeDailyWageService dailyWageService) 
		{
			_employeeService = employeeService;
			_logger = logger;
			_dailyWageService = dailyWageService;
		}
		
		[HttpGet("employees")]
		public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
		{
			_logger.LogInformation("Employee listesi sorgulanıyor");
			var employees = await _employeeService.GetAllAsync("sp_GetEmployees");
			return Ok(employees);
		}

		[HttpGet("employees/{id}")]
		public async Task<ActionResult<Employee>> GetById(int id)
		{
			var employee = await _employeeService.GetByIdAsync("sp_GetEmployees", id, "Id");
			if (employee == null) return NotFound();
			return Ok(employee);
		}

		[HttpPost("employees")]
		public async Task<ActionResult<Employee>> Create(Employee employee)
		{
			var result = await _employeeService.AddAsync(employee, "sp_AddEmployee");
			// CreatedAtAction için ID'nin repository veya SP tarafından set edilmesi gerekir
			return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
		}

		[HttpGet("payrolls/{month}")]
		public async Task<ActionResult<Employee>> GetPayrolls(string month)
		{
			_logger.LogInformation("Employee listesi sorgulanıyor");
			var employee = await _employeeService.GetEmployeePeriodicPayroll("sp_GetEmployeePeriodicPayroll", month);
			if (employee == null) return NotFound();
			_logger.LogInformation("Employee listesi sorgu sonu başarılı");
			return Ok(employee);
		}

		[HttpPost("payrolls")]
		public async Task<ActionResult<EmployeeDailyWage>> PostPayrolls([FromBody] EmployeeDailyWage employeeDailyWage)
		{
			_logger.LogInformation("Çalışan için ilgili güne kayıt atılıyor..");
			var employeeWage = await _dailyWageService.AddAsync(employeeDailyWage, "sp_AddEmployeeWorkTimeForDate");
			if (employeeWage == null) return NotFound();
			_logger.LogInformation("Kayıt edildi");
			return Ok(employeeWage);
		}

		[HttpGet("worklogs")]
		public async Task<ActionResult<EmployeeDailyWage>> GetWorkHistory()
		{
			_logger.LogInformation("Çalışma geçmişi isteği");
			var employeeWage = await _dailyWageService.GetAllAsync("sp_WorkLogs");
			if (employeeWage == null) return NotFound();
			_logger.LogInformation("Kayıt edildi");
			return Ok(employeeWage);
		}
	}
}
