using CgbSteps.Application.Abstractions;
using CgbSteps.Domain.Time;

namespace CgbSteps.Application.Status;

public sealed record SystemInfo(string Version, DateTimeOffset ServerTimeUtc, DateOnly QatarDate);

public sealed class GetSystemInfoHandler(TimeProvider clock, IApplicationInfo applicationInfo)
{
    public SystemInfo Handle()
    {
        var now = clock.GetUtcNow();
        return new SystemInfo(applicationInfo.Version, now, QatarTime.ToQatarDate(now));
    }
}
