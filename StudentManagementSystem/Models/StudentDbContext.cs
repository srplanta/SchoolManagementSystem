using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models;

public partial class StudentDbContext : DbContext
{
    public StudentDbContext()
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FeeTransaction> FeeTransactions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=DESKTOP-MAE99H0; database=StudentDb; trusted_connection=true; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeeTransaction>(entity =>
        {
            entity.HasKey(e => e.FeeId);

            entity.ToTable("FeeTransaction");

            entity.Property(e => e.AdmissionFee).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.FeePaid).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.FeePayable).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Fine).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.NextArrears).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.PreviousArrears).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.StationaryCharges).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.TutionFee).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Student).WithMany(p => p.FeeTransactions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeeTransaction_Student");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StdId);

            entity.ToTable("Student");

            entity.Property(e => e.StdId).HasColumnName("std_Id");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
