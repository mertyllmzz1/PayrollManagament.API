using System;
using System.Collections.Generic;

namespace PayrollManagement.Data.Models;

public partial class EmployeeDailyWage
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int Year { get; set; }

    public short Month { get; set; }

    public short Day { get; set; }

    public int DailyTotalWorkingMinute { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
