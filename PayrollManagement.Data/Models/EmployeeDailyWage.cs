using PayrollManagement.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PayrollManagement.Data.Models;

public partial class EmployeeDailyWage
{
	[IgnorePropertyAttirubute]
	[JsonIgnore]
	public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int Year { get; set; }

    public short Month { get; set; }

    public short Day { get; set; }

    public int DailyTotalWorkingMinute { get; set; }
	[IgnorePropertyAttirubute]
	public DateTime CreatedAt { get; set; }


}
