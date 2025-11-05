using PayrollManagement.Data;
using PayrollManagement.Data.Abstracts;
using PayrollManagement.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DataContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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
