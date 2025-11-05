using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagement.Data.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class IgnorePropertyAttirubute : Attribute
	{
	}
}
