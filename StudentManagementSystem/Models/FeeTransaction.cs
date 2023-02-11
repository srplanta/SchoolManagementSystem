using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Models;

public partial class FeeTransaction
{
    public int FeeId { get; set; }

    public int StudentId { get; set; }

    public decimal TutionFee { get; set; }

    public decimal? PreviousArrears { get; set; }

    public decimal? AdmissionFee { get; set; }

    public decimal? Fine { get; set; }

    public decimal? StationaryCharges { get; set; }

    public decimal? FeePayable { get; set; }

    public decimal? FeePaid { get; set; }

    public decimal? NextArrears { get; set; }

    public virtual Student Student { get; set; } = null!;
}
