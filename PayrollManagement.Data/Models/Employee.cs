using PayrollManagement.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PayrollManagement.Data.Models;

public partial class Employee
{
    [JsonIgnore]
	[IgnorePropertyAttirubute]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public short PayrollType { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool Active { get; set; }

    public decimal DailyWage { get; set; }

    public string IdentityNo { get; set; } = null!;

}
