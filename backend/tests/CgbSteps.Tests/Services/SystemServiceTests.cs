using CgbSteps.Api.Services;
using Microsoft.Extensions.Time.Testing;

namespace CgbSteps.Tests.Services;

public sealed class SystemServiceTests
{
    [Fact]
    public void GetInfo_ReturnsVersionAndQatarDate()
    {
        var clock = new FakeTimeProvider(new DateTimeOffset(2026, 11, 27, 22, 30, 0, TimeSpan.Zero));
        var service = new SystemService(clock);

        var info = service.GetInfo();

        Assert.False(string.IsNullOrWhiteSpace(info.Version));
        Assert.Equal(clock.GetUtcNow(), info.ServerTimeUtc);
        Assert.Equal(new DateOnly(2026, 11, 28), info.QatarDate);
    }
}
