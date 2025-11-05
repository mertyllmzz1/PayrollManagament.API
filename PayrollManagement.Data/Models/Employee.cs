using System;
using System.Collections.Generic;

namespace PayrollManagement.Data.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public short PayrollType { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Active { get; set; }

    public decimal DailyWage { get; set; }

    public string IdentityNo { get; set; } = null!;

    public virtual ICollection<EmployeeDailyWage> EmployeeDailyWages { get; set; } = new List<EmployeeDailyWage>();

    public virtual PayrollType PayrollTypeNavigation { get; set; } = null!;
}
