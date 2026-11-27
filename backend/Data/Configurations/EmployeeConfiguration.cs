using CgbSteps.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CgbSteps.Api.Data.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeId).HasMaxLength(20).IsRequired();
        builder.HasIndex(e => e.EmployeeId).IsUnique();

        builder.Property(e => e.FullName).HasMaxLength(200).IsRequired();

        builder.Property(e => e.WorkEmail).HasMaxLength(254).IsRequired();
        builder.HasIndex(e => e.WorkEmail).IsUnique();

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
