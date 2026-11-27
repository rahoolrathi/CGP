using CgbSteps.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CgbSteps.Api.Data;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.TraversePath().NoClobber().Load();
        var url = Environment.GetEnvironmentVariable(AppConstants.DatabaseKeys.Migrations)
            ?? "postgresql://postgres@localhost/cgb_steps";
        var connectionString = DatabaseUrlHelper.ToConnectionString(url);

        var options = new DbContextOptionsBuilder<AppDbContext>();
        AppDbContext.Configure(options, connectionString);
        return new AppDbContext(options.Options);
    }
}
