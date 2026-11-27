using CgbSteps.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CgbSteps.Api.Data.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(d => d.Name).IsUnique();
    }
}
