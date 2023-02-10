using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Models;

public partial class FeeTransaction
{
    public int FeeId { get; set; }

    public int StudentId { get; set; }

    public decimal TutionFee { get; set; } = 0.0m;

    public decimal? PreviousArrears { get; set; } = 0.0m;

    public decimal? AdmissionFee { get; set; } = 0.0m;

    public decimal? Fine { get; set; } = 0.0m;

    public decimal? StationaryCharges { get; set; } = 0.0m;

    public decimal? FeePayable { get; set; } = 0.0m;

    public decimal? FeePaid { get; set; } = 0.0m;

    public decimal? NextArrears { get; set; } = 0.0m;

    public virtual Student Student { get; set; } = null!;
}
