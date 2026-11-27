using System.Globalization;
using CgbSteps.Api.Helpers;

namespace CgbSteps.Tests.Helpers;

public sealed class QatarTimeHelperTests
{
    [Theory]
    [InlineData("2026-11-27T20:59:59Z", "2026-11-27")]
    [InlineData("2026-11-27T21:00:00Z", "2026-11-28")]
    [InlineData("2026-11-27T00:00:00Z", "2026-11-27")]
    public void ToQatarDate_UsesQatarMidnight(string instantUtc, string expected)
    {
        var date = QatarTimeHelper.ToQatarDate(DateTimeOffset.Parse(instantUtc, CultureInfo.InvariantCulture));

        Assert.Equal(DateOnly.Parse(expected, CultureInfo.InvariantCulture), date);
    }

    [Fact]
    public void DayBoundsUtc_CoversOneQatarDay()
    {
        var (start, end) = QatarTimeHelper.DayBoundsUtc(new DateOnly(2026, 11, 27));

        Assert.Equal(new DateTimeOffset(2026, 11, 26, 21, 0, 0, TimeSpan.Zero), start);
        Assert.Equal(TimeSpan.FromDays(1), end - start);
    }
}
