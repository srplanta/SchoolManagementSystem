using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Models;

public partial class Student
{
    public int StdId { get; set; }

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public int Standard { get; set; }

    public virtual ICollection<FeeTransaction> FeeTransactions { get; } = new List<FeeTransaction>();
}
