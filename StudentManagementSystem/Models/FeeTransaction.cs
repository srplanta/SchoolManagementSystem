using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models;

[Table("FeeTransaction")]
public partial class FeeTransaction
{
    [Key]
    public int FeeId { get; set; }

    public int StudentId { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal TutionFee { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? PreviousArrears { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? AdmissionFee { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? Fine { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? StationaryCharges { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? FeePayable { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? FeePaid { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal? NextArrears { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("FeeTransactions")]
    public virtual Student Student { get; set; } = null!;
}
