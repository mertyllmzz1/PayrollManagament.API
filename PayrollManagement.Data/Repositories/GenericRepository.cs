using Microsoft.Data.SqlClient;
using PayrollManagement.Data.Abstracts;
using PayrollManagement.Extentions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PayrollManagement.Data.Attributes;
namespace PayrollManagement.Data.Repositories
{
	public class GenericRepository<T>:IGenericRepository<T> where T : class, new()
	{
		private readonly DataContext _context;
		private readonly IDbConnection _connection;
		public GenericRepository(DataContext context)
		{
			_context = context;
			_connection = _context.CreateConnection();
		}

		public async Task<int> AddAsync(T entity, string spName)
		{
				if (_connection.State != ConnectionState.Open)
			{
				if (_connection is System.Data.Common.DbConnection dbConn)
					await dbConn.OpenAsync();
				else
					_connection.Open();
			}

			// Insert edilecek propertyleri seç
			var props = typeof(T).GetProperties()
				.Where(p => !p.GetMethod!.IsVirtual) // virtual olanları atla
				.Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string)) // ICollection vs atla
				.Where(p => p.GetCustomAttribute<IgnorePropertyAttirubute>() == null)
				.ToArray();

			// Kolon isimleri ve parametre isimleri
			var columnNames = string.Join(", ", props.Select(p => p.Name));
			var paramNames = string.Join(", ", props.Select(p => "@" + p.Name));

			//var sql = $"INSERT INTO {spName} {paramNames}";
			var sql = $"exec {spName} {paramNames}";

			using var command = _connection.CreateCommand();
			command.CommandText = sql;

			foreach (var prop in props)
			{
				var value = prop.GetValue(entity) ?? DBNull.Value;

				var parameter = command.CreateParameter();
				parameter.ParameterName = "@" + prop.Name;
				parameter.Value = value;

				command.Parameters.Add(parameter);
			}
				return await ((DbCommand)command).ExecuteNonQueryAsync();
			
		}

		public async Task<bool> DeleteAsync(string tableName, string idColumn, int id)
		{
			using var connection = _context.CreateConnection(); 
			var sql = $"DELETE FROM {tableName} WHERE {idColumn}=@id";
			using var command = new SqlCommand(sql, (SqlConnection)connection);
			command.Parameters.AddWithValue("@id", id);
		    StartConnection(connection);
			return await command.ExecuteNonQueryAsync() > 0;
		}

		public async Task<IEnumerable<T>> GetAllAsync(string spName)
		{
			using var connection = _context.CreateConnection();
			using var command = new SqlCommand($"Exec {spName}", (SqlConnection)connection);
			await StartConnection(connection);

			var list = new List<T>();
			using var reader = await command.ExecuteReaderAsync();
			var properties = typeof(T).GetProperties();

			while (await reader.ReadAsync())
			{
				var entity = new T();
				foreach (var prop in properties)
				{
					if (!reader.HasColumn(prop.Name) || reader[prop.Name] is DBNull)
						continue;
					prop.SetValue(entity, reader[prop.Name]);
				}
				list.Add(entity);
			}

			return list;
		}

		public async Task<T?> GetByIdAsync(string spName, int id, string idColumn)
		{
			using var connection = _context.CreateConnection();
			using var command = new SqlCommand($"exec {spName}", (SqlConnection)connection);
			command.Parameters.AddWithValue("@id", id);
			await StartConnection(connection);

			using var reader = await command.ExecuteReaderAsync();
			var properties = typeof(T).GetProperties();

			if (await reader.ReadAsync())
			{
				var entity = new T();
				foreach (var prop in properties)
				{
					if (!reader.HasColumn(prop.Name) || reader[prop.Name] is DBNull)
						continue;
					prop.SetValue(entity, reader[prop.Name]);
				}
				return entity;
			}

			return null;
		}

		public async Task<bool> UpdateAsync(string spName,T entity)
		{
			if (_connection.State != ConnectionState.Open)
			{
				if (_connection is System.Data.Common.DbConnection dbConn)
					await dbConn.OpenAsync();
				else
					_connection.Open();
			}

			var props = typeof(T).GetProperties()
				.Where(p => !p.GetMethod!.IsVirtual) // virtual olanları atla
				.Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType) || p.PropertyType == typeof(string)) // ICollection vs atla
				//.Where(p => p.GetCustomAttribute<IgnorePropertyAttirubute>() == null)
				.ToArray();
			using var connection = _context.CreateConnection();

			var paramNames = string.Join(", ", props.Select(p => "@" + p.Name));
			var sql = $"exec {spName} {paramNames}";


			using var command = _connection.CreateCommand();
			command.CommandText = sql;

			foreach (var prop in props)
			{
				var value = prop.GetValue(entity) ?? DBNull.Value;

				var parameter = command.CreateParameter();
				parameter.ParameterName = "@" + prop.Name;
				parameter.Value = value;

				command.Parameters.Add(parameter);
			}
			return await ((DbCommand)command).ExecuteNonQueryAsync() >0;
		}
		private async Task StartConnection(IDbConnection? connection)
		{
			await ((SqlConnection)connection).OpenAsync();
		}
	}
}
