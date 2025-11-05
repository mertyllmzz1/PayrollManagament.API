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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.Run();

//app.UseAuthorization();
app.MapControllers();
app.Run();
