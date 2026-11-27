using CgbSteps.Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CgbSteps.Api.Data;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable($"ConnectionStrings__{AppConstants.ConnectionStrings.Migrations}")
            ?? "Host=localhost;Database=cgb_steps;Username=postgres";

        var options = new DbContextOptionsBuilder<AppDbContext>();
        AppDbContext.Configure(options, connectionString);
        return new AppDbContext(options.Options);
    }
}
