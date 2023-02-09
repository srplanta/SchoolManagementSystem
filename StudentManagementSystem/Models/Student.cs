using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Models;

public partial class Student
{
    public int StdId { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public int? Age { get; set; }

    public int? Standard { get; set; }
}
