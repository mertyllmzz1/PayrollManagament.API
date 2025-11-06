using Microsoft.AspNetCore.Mvc;
using PayrollManagement.Data.Models;
using PayrollManagement.Service.Abstracts;

namespace PayrollManagament.API.Controllers
{
	[Route("api/Employee")]
	[ApiController]
	public class EmployeeController : Controller
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
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
		{
			_logger.LogInformation("Employee listesi sorgulanıyor");
			var employees = await _employeeService.GetAllAsync("GetEmployees");
			return Ok(employees);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Employee>> GetById(int id)
		{
			var employee = await _employeeService.GetByIdAsync("GetEmployees", id, "Id");
			if (employee == null) return NotFound();
			return Ok(employee);
		}

		[HttpPost]
		public async Task<ActionResult<Employee>> Create(Employee employee)
		{
			var result = await _employeeService.AddAsync(employee, "AddEmployee");
			// CreatedAtAction için ID'nin repository veya SP tarafından set edilmesi gerekir
			return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
		}

		[HttpGet("payrolls/{month}")]
		public async Task<ActionResult<Employee>> GetPayrolls(string month)
		{
			_logger.LogInformation("Employee listesi sorgulanıyor");
			var employee = await _employeeService.GetEmployeePeriodicPayroll("GetEmployeePeriodicPayroll", month);
			if (employee == null) return NotFound();
			_logger.LogInformation("Employee listesi sorgu sonu başarılı");
			return Ok(employee);
		}

		[HttpPost("payrolls")]
		public async Task<ActionResult<EmployeeDailyWage>> GetPayrolls([FromBody] EmployeeDailyWage employeeDailyWage)
		{
			_logger.LogInformation("Çalışan için ilgili güne kayıt atılıyor..");
			var employeeWage = await _dailyWageService.AddAsync(employeeDailyWage,"AddEmployeeWorkTimeForDate");
			if (employeeWage == null) return NotFound();
			_logger.LogInformation("Kayıt edildi");
			return Ok(employeeWage);
		}
	}
}
