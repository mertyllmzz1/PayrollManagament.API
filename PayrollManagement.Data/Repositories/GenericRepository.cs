using PayrollManagement.Data.Abstracts;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PayrollManagement.Extentions;
namespace PayrollManagement.Data.Repositories
{
	public class GenericRepository<T>:IGenericRepository<T> where T : class, new()
	{
		private readonly DataContext _context;
		public GenericRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<int> AddAsync(string tableName, Dictionary<string, object> parameters)
		{
			using var connection = _context.CreateConnection();
			var columns = string.Join(", ", parameters.Keys);
			var values = string.Join(", ", parameters.Keys.Select(k => "@" + k));

			var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
			using var command = new SqlCommand(sql, (SqlConnection)connection);

			foreach (var p in parameters)
				command.Parameters.AddWithValue("@" + p.Key, p.Value ?? DBNull.Value);

			StartConnection(connection);
			return await command.ExecuteNonQueryAsync();
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

		public async Task<IEnumerable<T>> GetAllAsync(string tableName)
		{
			using var connection = _context.CreateConnection();
			using var command = new SqlCommand($"SELECT * FROM {tableName}", (SqlConnection)connection);
			StartConnection(connection);

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

		public async Task<T?> GetByIdAsync(string tableName, int id, string idColumn)
		{
			using var connection = _context.CreateConnection();
			using var command = new SqlCommand($"SELECT * FROM {tableName} WHERE {idColumn} = @id", (SqlConnection)connection);
			command.Parameters.AddWithValue("@id", id);
			StartConnection(connection);

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

		public async Task<bool> UpdateAsync(string tableName, Dictionary<string, object> parameters, string idColumn, int id)
		{
			using var connection = _context.CreateConnection();
			var setClause = string.Join(", ", parameters.Keys.Select(k => $"{k}=@{k}"));
			var sql = $"UPDATE {tableName} SET {setClause} WHERE {idColumn}=@id";

			using var command = new SqlCommand(sql, (SqlConnection)connection);
			foreach (var p in parameters)
				command.Parameters.AddWithValue("@" + p.Key, p.Value ?? DBNull.Value);

			command.Parameters.AddWithValue("@id", id);
			StartConnection(connection);
			return await command.ExecuteNonQueryAsync() > 0;
		}
		private async void StartConnection(IDbConnection? connection)
		{
			await ((SqlConnection)connection).OpenAsync();
		}
	}
}
