using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PayrollManagement.Data.Models;

public partial class EmployeeManagementContext : DbContext
{
    public EmployeeManagementContext()
    {
    }

    public EmployeeManagementContext(DbContextOptions<EmployeeManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeDailyWage> EmployeeDailyWages { get; set; }

    public virtual DbSet<PayrollType> PayrollTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EmployeeManagement;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07728B92BD");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DailyWage).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IdentityNo)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasOne(d => d.PayrollTypeNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PayrollType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PayrollType");
        });

        modelBuilder.Entity<EmployeeDailyWage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07E136D6FD");

            entity.Property(e => e.CreatedAt).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeDailyWages)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeD__Emplo__34C8D9D1");
        });

        modelBuilder.Entity<PayrollType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PayrollT__3214EC073B660BD3");

            entity.Property(e => e.Description).HasMaxLength(300);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
