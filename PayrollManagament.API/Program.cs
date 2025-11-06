using PayrollManagement.Data;
using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Repositories;
using PayrollManagement.Service.Abstracts;
using PayrollManagement.Service.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeeService,EmployeeService>();
builder.Services.AddScoped<IEmployeeDailyWageService, EmployeeDailyWageService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigins", policy =>
	{
		policy.WithOrigins(
			"https://app.myweb.com",    
			"https://setyazilim.com.tr"                     
		)
		.AllowAnyHeader()
		.AllowAnyMethod();
	});


	options.AddPolicy("AllowDev", policy =>
	{
		policy
			.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseCors("AllowDev");
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
	app.UseCors("AllowSpecificOrigins");

//app.UseAuthorization();
app.MapControllers();
app.Run();
