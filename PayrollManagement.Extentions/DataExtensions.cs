using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Extentions
{
	public static class DataExtensions
	{
		//Gerek kalmadı ama ilerde lazım olabilir. Hascolum SqlDAtaReader içinde bir kolonun var olup olmadığını kontrol eden metot varmış.
		public static bool HasColumn(this SqlDataReader reader, string columnName)
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
				if (reader.GetName(i).Equals(columnName,  StringComparison.InvariantCultureIgnoreCase))
					return  true;
			}
			return false;
		}

		public static Dictionary<string, object> ToDictionary(object obj)
		{
			var dict = new Dictionary<string, object>();

			if (obj == null)
				return dict;

			foreach (PropertyInfo prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				dict[prop.Name] = prop.GetValue(obj, null);
			}

			return dict;
		}

	}
}
