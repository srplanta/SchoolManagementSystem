using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models;

[Table("Student")]
public partial class Student
{
    [Key]
    [Column("std_Id")]
    public int StdId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(10)]
    public string? Gender { get; set; }

    public int? Age { get; set; }

    public int Standard { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<FeeTransaction> FeeTransactions { get; } = new List<FeeTransaction>();
}
