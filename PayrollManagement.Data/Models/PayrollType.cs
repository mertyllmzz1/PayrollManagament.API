using System;
using System.Collections.Generic;

namespace PayrollManagement.Data.Models;

public partial class PayrollType
{
    public short Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
