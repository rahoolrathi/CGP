using CgbSteps.Api.Helpers;
using CgbSteps.Api.Services.Interfaces;
using CgbSteps.Api.Models.Dtos;

namespace CgbSteps.Api.Services;

public sealed class SystemService(TimeProvider clock) : ISystemService
{
    public SystemInfoResponse GetInfo()
    {
        var now = clock.GetUtcNow();
        return new SystemInfoResponse(AppVersion.Current, now, QatarTimeHelper.ToQatarDate(now));
    }
}
