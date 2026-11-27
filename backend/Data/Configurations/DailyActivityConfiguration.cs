using CgbSteps.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CgbSteps.Api.Data.Configurations;

public sealed class DailyActivityConfiguration : IEntityTypeConfiguration<DailyActivity>
{
    public void Configure(EntityTypeBuilder<DailyActivity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Source).HasMaxLength(100);

        // One row per employee per day; also the index for reading an employee's history.
        builder.HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();

        builder.HasOne<Employee>()
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
