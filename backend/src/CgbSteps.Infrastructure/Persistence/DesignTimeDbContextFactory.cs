using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CgbSteps.Infrastructure.Persistence;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Migrations")
            ?? "Host=localhost;Database=cgb_steps;Username=postgres";

        var options = new DbContextOptionsBuilder<AppDbContext>();
        DbContextOptionsFactory.Configure(options, connectionString);
        return new AppDbContext(options.Options);
    }
}
