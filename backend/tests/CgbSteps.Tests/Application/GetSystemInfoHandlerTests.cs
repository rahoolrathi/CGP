using CgbSteps.Application.Abstractions;
using CgbSteps.Application.Status;
using Microsoft.Extensions.Time.Testing;

namespace CgbSteps.Tests.Application;

public sealed class GetSystemInfoHandlerTests
{
    private sealed class FixedApplicationInfo : IApplicationInfo
    {
        public string Version => "1.2.3";
    }

    [Fact]
    public void Handle_ReturnsVersionAndQatarDate()
    {
        var clock = new FakeTimeProvider(new DateTimeOffset(2026, 11, 27, 22, 30, 0, TimeSpan.Zero));
        var handler = new GetSystemInfoHandler(clock, new FixedApplicationInfo());

        var info = handler.Handle();

        Assert.Equal("1.2.3", info.Version);
        Assert.Equal(clock.GetUtcNow(), info.ServerTimeUtc);
        Assert.Equal(new DateOnly(2026, 11, 28), info.QatarDate);
    }
}
