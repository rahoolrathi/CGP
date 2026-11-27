using CgbSteps.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CgbSteps.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    internal static void Configure(DbContextOptionsBuilder options, string connectionString)
    {
        options
            .UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure())
            .UseSnakeCaseNamingConvention();
    }
}
